var geocoder = new google.maps.Geocoder();
var infowindow = new google.maps.InfoWindow();
var marker;
var resultList;
var service;
var map;
var elevator;
var layer;

$(document).ready(function() {
    google.maps.event.addDomListener(window, 'load', initialize);
    $('#nearbyAirports').on('show.bs.modal', function(event) {
        var button = $(event.relatedTarget); // Button that triggered the modal
        //var recipient = button.data('whatever'); // Extract info from data-* attributes
        //// If necessary, you could initiate an AJAX request here (and then do the updating in a callback).
        //// Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.
        //var modal = $(this);
        //modal.find('.modal-title').text('New message to ' + recipient);
        //modal.find('.modal-body input').val(recipient);
        var latlng = new google.maps.LatLng(button.data("latitude"), button.data("longitude"));
        clickMap(latlng);
    });
    $('#nearbyAirports').on('hide.bs.modal', function(event) {
        var modal = $(this);
        var $list = modal.find('.panel-group');
        var data = $list.find(".in").children(".panel-body");
        if (typeof marker != 'undefined') {
            marker.setMap(null);
        }
        $("#Name").val(data.data("name"));
        $("#Country").val(data.find(".country-name").text());
        $("#StateOrProvidence").val(data.find(".region").text());
        $("#City").val(data.find(".locality").text());
        $("#Latitude").val(data.data("latitude"));
        $("#Longitude").val(data.data("longitude"));
        $("#Timezone").val(data.data("timezone"));
        $("#Altitude").val(data.data("altitude"));
        createMarker(data.data("info"));
        $list.empty();
    });
});

function initialize() {
    var mapOptions = {
        center: { lat: 38.228, lng: -85.663722 },
        zoom: 8
    };
    map = new google.maps.Map(document.getElementById('map-canvas'),
        mapOptions);

    var contextMenu = google.maps.event.addListener(
        map,
        'rightclick', function(e) {
            var $contextMenu = $("#contextMenu");
            $contextMenu.css({
                display: "block",
                left: e.pixel.x,
                top: e.pixel.y
            });
            $contextMenu.find("#nearbyAirportsLink").data('latitude', e.latLng.lat());
            $contextMenu.find("#nearbyAirportsLink").data('longitude', e.latLng.lng());
            $("body").on("click", function() {
                $contextMenu.hide();
            });
        }
    );
};

function clickMap(latlng) {
    var request = {
        location: latlng,
        radius: 50000,
        types: ['airport']
    };
    infowindow = new google.maps.InfoWindow();
    service = new google.maps.places.PlacesService(map);
    service.nearbySearch(request, callback);

};

function callback(results, status) {
    if (status === google.maps.places.PlacesServiceStatus.OK) {
        $(results).each(function(e) {
            if (e < 10) {

                var requestDetails = {
                    placeId: this.place_id
                };
                service.getDetails(requestDetails, function(place, status) {
                    if (status === google.maps.places.PlacesServiceStatus.OK) {
                        layer = new google.maps.FusionTablesLayer({
                            query: {
                                select: '\'ident\'',
                                from: '1BL1dqutGsIUgQweywP6V6d7CWEKbtJxbgQHILd4',
                                where: 'type IN (\'large_airport\', \'medium_airport\', \'small_airport\')'
                            }
                        });
                        var $panelGroup = $("#nearbyAirports").find(".panel-group");
                        $panelGroup.append(createPanel(place, e));
                        var $panel = $panelGroup.children().last().find(".panel-body");
                        $panel.html(createPanelContent(place));
                        $panel.data("latitude", place.geometry.location.G);
                        $panel.data("longitude", place.geometry.location.K);
                        $panel.data("id", place.id);
                        $panel.data("place_id", place.place_id);
                        $panel.data("timezone", place.utc_offset / 60);
                        $panel.data("info", place);
                        $panel.data("name", place.name);
                        elevator = new google.maps.ElevationService();
                        var locations = [];
                        locations.push(new google.maps.LatLng(place.geometry.location.G, place.geometry.location.K));

                        // Create a LocationElevationRequest object using the array's one value
                        var positionalRequest = {
                            'locations': locations
                        }; // Initiate the location request
                        elevator.getElevationForLocations(positionalRequest, function(results, status) {
                            if (status == google.maps.ElevationStatus.OK) {

                                // Retrieve the first result
                                if (results[0]) {
                                    var feet = results[0].elevation * 3.28084;
                                    $panel.data("altitude", Math.round(feet));
                                } else {
                                    alert('No results found');
                                }
                            } else {
                                alert('Elevation service failed due to: ' + status);
                            }
                        });


                    }
                });


            }
        });
    }
}


function getElevation(event) {

    var locations = [];

    // Retrieve the clicked location and push it on the array
    var clickedLocation = event.latLng;
    locations.push(clickedLocation);

    // Create a LocationElevationRequest object using the array's one value
    var positionalRequest = {
        'locations': locations
    }; // Initiate the location request
    elevator.getElevationForLocations(positionalRequest, function(results, status) {
        if (status == google.maps.ElevationStatus.OK) {

            // Retrieve the first result
            if (results[0]) {

                // Open an info window indicating the elevation at the clicked position
                infowindow.setContent('The elevation at this point <br>is ' + results[0].elevation + ' meters.');
                infowindow.setPosition(clickedLocation);
                infowindow.open(map);
            } else {
                alert('No results found');
            }
        } else {
            alert('Elevation service failed due to: ' + status);
        }
    });
}

function createPanel(place, e) {
    var panel = "<div class='panel panel-default'>" +
        "<div class='panel-heading' role='tab' id='heading" + e + "'>" +
        "<h4 class='panel-title'>" +
        "<a role='button' data-toggle='collapse' data-parent='#accordion' href='#collapse" + e + "' aria-expanded='false' aria-controls='collapse" + e + "'>" +
        place.name +
        "</a>" +
        "</h4>" +
        "</div>" +
        "<div id='collapse" + e + "' class='panel-collapse collapse' role='tabpanel' aria-labelledby='heading" + e + "'>" +
        "<div class='panel-body'>" +
        "</div>" +
        "</div>" +
        "</div>";
    return panel;
}

function createPanelContent(place) {
    var content = "<div class='container-fluid'>" +
        "<div class='row'>" +
        "<div class='col-sm-6'>" +
        "<table class='table table-responsive'>" +
        "<tr>" +
        "<td>Address</td>" +
        "<td class='address'>" + place.adr_address + "</td>" +
        "</tr>" +
        "<tr>" +
        "<td>Latitude</td>" +
        "<td>" + place.geometry.location.G + "</td>" +
        "</tr>" +
        "<tr>" +
        "<td>Longitude</td>" +
        "<td>" + place.geometry.location.K + "</td>" +
        "</tr>" +
        "</table>" +
        "</div>" +
        "<div class='col-sm-6'>";
    if (typeof place.photos == 'undefined') {
        content += "<div style='height:200px; text-align:center; line-height:200px;'>" +
            "<img style='display:inline-block; vertical-align:middle;' src='" + place.icon + "' />" +
            "</div>";
    } else if (place.photos.length === 1) {
        content += "<div style='height:200px; text-align:center; line-height:200px;'>" +
            "<img style='display:inline-block; vertical-align:middle;' src='" + place.photos[0].getUrl({ 'maxWidth': 250, 'maxHeight': 200 }) + "' />" +
            "</div>";
    } else {
        content += createPanelCarousel(place.place_id, place.photos);
    }
    content += "</div>" +
        "<div class='col-sm-6 text-center' style='clear:left;'>" +
        "<button type='button' class='btn btn-default text-center'>Add Airport</button>" +
        "</div>" +
        "</div>" +
        "</div>";
    return content;
}

function createPanelCarousel(id, photos) {
    var carousel = "<div id='carousel-" + id + "' class='carousel slide' data-ride='carousel'>" +
        "<div class='carousel-inner' role='listbox'>";
    $(photos).each(function(e) {
        var photo = "<div class='item text-center' style='height:200px; line-height:200px;'>";
        if (e === 0) {
            photo = "<div class='item active text-center' style='height:200px; line-height:200px;'>";
        }

        photo += "<img style='display:inline-block;' src='" + this.getUrl({ 'maxWidth': 250, 'maxHeight': 200 }) + "' />" +
            "</div>";
        carousel += photo;
    });
    carousel += "</div>" +
        "<a class='left carousel-control' href='#carousel-" + id + "' role='button' data-slide='prev'>" +
        "<span class='glyphicon glyphicon-chevron-left' aria-hidden='true'></span>" +
        "<span class='sr-only'>Previous</span>" +
        "</a>" +
        "<a class='right carousel-control' href='#carousel-" + id + "' role='button' data-slide='next'>" +
        "<span class='glyphicon glyphicon-chevron-right' aria-hidden='true'></span>" +
        "<span class='sr-only'>Next</span>" +
        "</a>" +
        "</div>";
    return carousel;
}

function createMarker(place) {
    var placeLoc = place.geometry.location;
    marker = new google.maps.Marker({
        map: map,
        position: place.geometry.location
    });

    google.maps.event.addListener(marker, 'click', function() {
        infowindow.setContent(place.name);
        infowindow.open(map, this);
    });
}

function cycleThroughGoogleMapResults(originalArray) {
    var address = originalArray;
    var establishmentNames = new Array();
    var streetNumberNames = new Array();
    var routeNames = new Array();
    var neighborhoodNames = new Array();
    var cityNames = new Array();
    var countyNames = new Array();
    var stateNames = new Array();
    var countryNames = new Array();
    var postalCodeNames = new Array();


    for (var i = 0; i < address.length; i++) {
        var ac = address[i].address_components;
        for (var j = 0; j < ac.length; j++) {
            var component = ac[j].types;
            for (var k = 0; k < component.length; k++) {
                if (component[k] === "establishment") LongShortAdd(establishmentNames, ac[j]);

                if (component[k] === "street_number") LongShortAdd(streetNumberNames, ac[j]);

                if (component[k] === "route") LongShortAdd(routeNames, ac[j]);

                if (component[k] === "neighborhood") LongShortAdd(neighborhoodNames, ac[j]);

                if (component[k] === "locality") LongShortAdd(cityNames, ac[j]);

                if (component[k] === "administrative_area_level_2") LongShortAdd(countyNames, ac[j]);

                if (component[k] === "administrative_area_level_1") LongShortAdd(stateNames, ac[j]);

                if (component[k] === "country") LongShortAdd(countryNames, ac[j]);

                if (component[k] === "postal_code") LongShortAdd(postalCodeNames, ac[j]);
            }
        }
    }
    var loc = new Object();
    loc.name = addProperty(establishmentNames);
    loc.streetNumber = addProperty(streetNumberNames);
    loc.streetName = addProperty(routeNames);
    loc.neighborhood = addProperty(neighborhoodNames);
    loc.city = addProperty(cityNames);
    loc.county = addProperty(countyNames);
    loc.state = addProperty(stateNames);
    loc.country = addProperty(countryNames);
    loc.postalCode = addProperty(postalCodeNames);

    return loc;

}

function checkIfNameIsInArray(array, name) {
    var check = true;
    for (var i = 0; i < array.length; i++) {
        if (array[i] == name) {
            check = false;
        }
    }
    return check;
}

function LongShortAdd(arrayNames, name) {
    if (checkIfNameIsInArray(arrayNames, name.long_name))
        arrayNames.push(name.long_name);
    if (checkIfNameIsInArray(arrayNames, name.short_name))
        arrayNames.push(name.short_name);
}

function addProperty(array) {
    if (array.length > 0) {
        for (var i = 0; i < array.length - 1; i++) {
            if (array[i].length > array[i + 1].length) {
                var temp = array[i];
                array[i] = array[i + 1];
                array[i + 1] = temp;
            }
        }
        return array[0];
    } else {
        return null;
    }
}