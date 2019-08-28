var infowindow = new google.maps.InfoWindow();
var elevator;

function makeMarker(lat, lng, name, airportCode, city, state, map) {

    var marker = new window.google.maps.Marker({
        position: new window.google.maps.LatLng(lat, lng),
        map: map,
        title: airportCode,
        icon: {
            path: window.google.maps.SymbolPath.CIRCLE,
            scale: 5,
            fillColor: 'black',
            fillOpacity: 1,
            strokeWeight: 0
        }
    });
    var content = name + "<br />" +
        city + ", " + state;
    createListener(marker, content, map);
    return marker;
}

function addMarker(location, map, name) {
    // Add the marker at the clicked location, and add the next-available label
    // from the array of alphabetical characters.
    var marker = new google.maps.Marker({
        position: location,
        icon: 'Scripts/img/pin/airport.png',
        map: map
    });

    createListener(marker, name, map);
}

function createFlightPath(depart, arrive, map, color) {
    var lineSymbol = {
        path: window.google.maps.SymbolPath.FORWARD_CLOSED_ARROW,
        strokeWeight: 0,
        scale: 3,
        fillOpacity: 1,
    };
    var arrow = {
        icon: lineSymbol,
        repeat: '25px',
        offset: '24px'
    };
    var flightPath = new window.google.maps.Polyline({
        path: [depart.position, arrive.position],
        geodesic: true,
        strokeColor: color,
        strokeOpacity: 1.0,
        strokeWeight: 3,
        icons: [arrow],
        map: map
    });
    return flightPath;
}

function createListener(mark, content, map) {
    window.google.maps.event.addListener(mark, "click", function(e) {
        infowindow.setContent(content);
        infowindow.setPosition({
            lat: e.latLng.lat(),
            lng: e.latLng.lng()
        });
        infowindow.open(map);

    });
}

function createDivListener(mark, content, map, div, middle) {
    window.google.maps.event.addListener(mark, "click", function(e) {
        infowindow.setContent(content);
        infowindow.setPosition({
            lat: e.latLng.lat(),
            lng: e.latLng.lng()
        });
        infowindow.open(map);

    });
    $(div).on("click", function() {
        infowindow.setContent(content);
        infowindow.setPosition({
            lat: middle.lat(),
            lng: middle.lng()
        });
        infowindow.open(map);
    });
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

function calcDistance(p1, p2) {
    return google.maps.geometry.spherical.computeDistanceBetween(p1, p2);
}

function Flight(depature, arrival, color, tailnumber, div) {
    this.depature = depature;
    this.arrival = arrival;
    this.color = color;
    this.tailnumber = tailnumber;
    this.div = div;
}