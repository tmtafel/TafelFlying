var indexStart;

function getNextWeek(date) {
    $.ajax({
        url: '@Url.Action("GetDay")',
        dataType: "json",
        type: "GET",
        contentType: 'application/json; charset=utf-8',
        async: true,
        data: { dateString: date },
        cache: false,
        success: function(data) {
            buildWeekDiv(data);
        },
        error: function(xhr) {
            alert('error');
        }
    });
}

$(document).ready(function() {
    $('.week-view').slick({
        autoplay: false,
        adaptiveHeight: true,
        infinite: false,
        mobileFirst: true,
        initialSlide: indexStart,
    });
    $('.week-view').on('afterChange', function(event, slick, currentSlide) {
        var lastSlideIndex = slick.slideCount - 1;
        if (currentSlide == 0) {
            alert("get previous week from db");
        }
        if (currentSlide == lastSlideIndex) {
            var $slide = $(slick.$slides.get(currentSlide));
            var dates = $slide.children("div");
            var lastDate = $(dates.last()).prop("id");
            getNextWeek(lastDate);
        }
    });
});

function buildWeekDiv(data) {
    var week = $.parseJSON(data);
    var sunday = buildDayDiv(week.Sunday, "Sunday");
    var monday = buildDayDiv(week.Monday, "Monday");
    var tuesday = buildDayDiv(week.Tuesday, "Tuesday");
    var wednesday = buildDayDiv(week.Wednesday, "Wednesday");
    var thursday = buildDayDiv(week.Thursday, "Thursday");
    var friday = buildDayDiv(week.Friday, "Friday");
    var saturday = buildDayDiv(week.Saturday, "Saturday");

    var allWeek = "<div>" + sunday + monday + tuesday + wednesday + thursday + friday + saturday + "</div>";
    $('.week-view').slick('slickAdd', allWeek);
}

function buildDayDiv(date, dow) {
    var flights = date.Flights;
    var flightArray = new Array();
    $(flights).each(function() {
        var color = this.Color;
        var tailnumber = this.TailNumber;
        var departure = this.Departure.AirportCode;
        var arrival = this.Arrival.AirportCode;
        var flight = new Flight(color, tailnumber, departure, arrival);
        flightArray.push(flight);
    });
    var dateString = date.Day;
    var dsa1 = dateString.split("t");
    var dsa2 = dsa1[0].split("-");
    var year = parseInt(dsa2[0]);
    var month = parseInt(dsa2[1]);
    var day = parseInt(dsa2[2]);
    var dv = new DayView(year, month, day, dow, flightArray);
    var dayDiv = createDayInDiv(dv);
    return dayDiv;
}

function createDayInDiv(dv) {
    var maindiv =
        "<div id='" + dv.id + "'>" +
            "<div class='single-day'>" +
            "<div class='date-title'>" +
            "<strong>" + dv.dayOfWeek + "" +
            "<span class='float-right'>" + dateString(dv.date) + "</span>" +
            "</strong>" +
            "</div>" +
            "</div>" +
            "</div>";
    return maindiv;
}

function dateString(date) {
    var months = { 0: "January", 1: "February", 2: "March", 3: "April", 4: "May", 5: "June", 6: "July", 7: "August", 8: "September", 9: "October", 10: "November", 11: "December" };
    var month = months[date.getMonth()];
    var stringDate = month + " " + date.getDate() + ", " + date.getFullYear();
    return stringDate;
}

function DayView(year, month, day, dayOfWeek, flights) {
    this.id = year.toString() + "-" + month.toString() + "-" + day.toString();
    this.date = new Date(year, month - 1, day, 0, 0, 0, 0);
    this.dayOfWeek = dayOfWeek;
    this.flights = flights;
}

function Flight(color, tailnumber, depart, arrive) {
    this.color = color;
    this.tailnumber = tailnumber;
    this.depart = depart;
    this.arrive = arrive;
}