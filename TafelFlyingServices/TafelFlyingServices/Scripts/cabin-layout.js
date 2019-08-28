/// <reference path="jquery-ui-1.11.4.js" />

$(function () {

	$("#save").on("click", function () {
		var url = $(this).data("save");
		window.saveLayout(url);
	});
	var $cabin = $("#cabin");
	var $seatModal = $('#seatModal');
	var $seatType = $('#seatTypeDropDown');

	$cabin.resizable({
		grid: [15, 15],
		handles: "e,w,s",
		minWidth: 60,
		minHeight: 60,
		maxWidth: 300,
		stop: function (event, ui) {
			var ccWidth = $('#cabinContainer').width();
			var width = ccWidth - (ccWidth % 15);

			if (width < ui.position.left + ui.size.width) {
				$cabin.css("left", width - ui.size.width);
			}
			else if (ui.position.left < 0) {
				$cabin.css("left", 0);
			}

		}
	});

	$cabin.draggable({
		cursor: "move",
		grid: [15, 15],
		axis: "x",
		containment: "#cabinContainer"
	});

	$(".seat", $cabin).draggable({
		cursor: "move",
		grid: [15, 15],
		containment: $("#cabin")
	});

	$(".seat", $cabin).each(function () {
		addDoubleClick($(this));
		addRightClick($(this));
	});

	$("#newSeat").draggable({
		cursor: "move",
		grid: [15, 15],
		revert: "invalid",
		containment: "#layout"
	});

	$cabin.droppable({
		hoverClass: "drop-hover",
		drop: function (event, ui) {
			addNewSeat(ui.draggable, $cabin);
		}
	});

	var directions = $("#seatModal .direction .btn");
	directions.on("click", function () {
		removeSelectClass();
		$(this).addClass("active");
		$(this).find(".seat-image").addClass("selected");
	});

	$("#update").on('click', function () {
		var direction = $seatModal.find(".direction .btn .seat-image.selected").parent(".btn").parent(".direction").attr("id");
		var $seat = $seatModal.data("selected");
		$seat.data("direction", direction);
		$seat.data("type", $seatModal.data('type'));
		$seat.attr("title", $seatModal.data('type'));
		$seatModal.modal('hide');
	});

	$("#delete").on('click', function () {
		var $seat = $seatModal.data("selected");
		deleteSeat($seat);
	});

	$("#dropdown-delete").on('click', function () {
		var $seat = $("#contextMenu").data("selected");
		deleteSeat($seat);
	});

	$("#dropdown-edit").on('click', function (e) {
		var $seat = $("#contextMenu").data("selected");
		$("#seatModal").data("selected", $seat);
		$("#seatModal").modal('show');
	});

	$("#face-up").on('click', function () {
		var $seat = $("#contextMenu").data("selected");
		$seat.data("direction", "up");
		setDirection($seat);
	});

	$("#face-down").on('click', function () {
		var $seat = $("#contextMenu").data("selected");
		$seat.data("direction", "down");
		setDirection($seat);
	});

	$("#face-right").on('click', function () {
		var $seat = $("#contextMenu").data("selected");
		$seat.data("direction", "right");
		setDirection($seat);
	});

	$("#face-left").on('click', function () {
		var $seat = $("#contextMenu").data("selected");
		$seat.data("direction", "left");
		setDirection($seat);
	});

	$seatModal.on('show.bs.modal', function (e) {
		var $modal = $(e.target);
		var $seat = $modal.data('selected');
		var seatType = $seat.data('type');
		if (seatType == null || seatType == "") {
			$modal.find("#selectedType").text("Choose Type");
		} else {
			$modal.find("#selectedType").text(seatType);
		}
		var direction = $seat.data('direction');
		var $modalFinder = $modal.find("#" + direction);
		var $modalFinderBtn = $modalFinder.find(".btn").addClass('active');
		$modalFinderBtn.find(".seat-image").addClass('selected');
	});

	$seatModal.on('hidden.bs.modal', function (e) {
		removeSelectClass();
		var $modal = $(e.target);
		var $seat = $modal.data('selected');
		setDirection($seat);
		$modal.data("selected", null);
	});

	$seatType.find('a').on('click', function () {
		var seatType = $(this).text();
		$("#selectedType").text(seatType);
		$seatModal.data('type', seatType);
	});
});

function addNewSeat($item, $cabin) {
	if ($item.attr("id") === "newSeat") {
		var $options = $("#options");
		var left = $item.offset().left - $cabin.offset().left;
		var top = $item.offset().top - $cabin.offset().top;
		$item.remove();

		$options.find(".new-seat-container").append("<span id='newSeat' style='display:none;'></span>");
		var $newSeat = $("#newSeat");
		$newSeat.draggable({
			cursor: "move",
			grid: [15, 15],
			revert: "invalid",
			containment: "#cabinContainer"
		});
		$newSeat.fadeIn();

		var index = $cabin.find(".seat").length;
		$cabin.append("<div class='seat'><div class='seat-image seat-direction-up'></div></div>");
		var $addSeat = $(".seat").last();
		$addSeat.data("index", index);
		$addSeat.css("left", left);
		$addSeat.css("top", top);
		$addSeat.css("position", "absolute");
		$addSeat.data("direction", "up");
		$addSeat.data("type", null);
		$addSeat.draggable({
			containment: $("#cabin"),
			cursor: "move",
			grid: [15, 15]
		});

		addDoubleClick($addSeat);
		addRightClick($addSeat);
	}
}

function addDoubleClick($item) {
	$item.dblclick(function (e) {
		var $seat = $(e.currentTarget);
		$("#seatModal").data("selected", $seat);
		$("#seatModal").modal('show');
	});
}

function addRightClick($item) {
	$item.mousedown(function (e) {
		if (e.button == 2) {
			document.oncontextmenu = function () { return false; };
			var $contextMenu = $("#contextMenu");
			$contextMenu.css({
				left: e.pageX,
				top: e.pageY
			});
			var $seat = $(e.currentTarget);
			$contextMenu.data('selected', $seat);
			$contextMenu.show(
                "fast",
                function () {
                	$("body").click(function (event) {
                		$contextMenu.hide(
                            "fast",
                            function () {
                            	document.oncontextmenu = function () { return true; };
                            });
                	});
                });
		}
		return true;
	});
}

function removeSelectClass() {
	var directions = $("#seatModal .direction .btn");
	directions.each(function () {

		$(this).removeClass('active');
		$(this).find(".seat-image").removeClass("selected");
	});
}

function getDirection() {
	var $modal = $("#seatModal");
	var direction = $modal.data("selected").data("direction");
	return direction;
}

function setDirection($item) {
	var direction = $item.data("direction");
	var $i = $item.find(".seat-image");
	$i.removeClass("seat-direction-up");
	$i.removeClass("seat-direction-down");
	$i.removeClass("seat-direction-left");
	$i.removeClass("seat-direction-right");
	var directionClass = "seat-direction-" + direction;
	$i.addClass(directionClass);
}

function deleteSeat($seat) {
	var index = $seat.data("index");
	var $seats = $("#cabin").find(".seat");
	$seats.each(function () {
		if ($(this).data("index") > index) {
			$(this).data("index", $(this).data("index") - 1);
		}
	});
	$seat.remove();
	$("#seatModal").modal('hide');
}

function removePX($string) {
	if ($string == "auto") {
		return 0;
	}

	var parsedInt = parseInt($string);
	return parsedInt;

}

function saveLayout(url) {
	var $cabin = $("#cabin");
	var $seats = $cabin.find(".seat");
	var seats = new Array();
	$seats.each(function () {
		var newSeat = new Object();
		newSeat.index = $(this).data("index");
		newSeat.left = removePX($(this).css("left"));
		newSeat.top = removePX($(this).css("top"));
		newSeat.direction = $(this).data("direction");
		newSeat.type = $(this).data("type");
		seats.push(newSeat);
	});

	var cabin = new Object();
	cabin.seats = seats;
	cabin.left = removePX($cabin.css("left"));
	cabin.top = removePX($cabin.css("top"));
	cabin.width = removePX($cabin.css("width"));
	cabin.height = removePX($cabin.css("height"));
	cabin.tailnumber = $("#tailnumber").text();
	var data = JSON.stringify(cabin);
	$.ajax({
		url: url,
		method: "POST",
		async: true,
		data: { data: data },
		cache: false,
		error: function (xhr) {
			$("#alertModal").find(".modal-body").html(xhr.responseText);
			$("#alertModal").modal('show');
		},
		complete: function () {
			//choices: success info warning danger
			var alertType = "success";
			var message = "Save Successful!!!";
			var alert = "<div style='display:none;' class='alert alert-info alert-" + alertType + " fade in' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>" + message + "</button></div>";
			$("body").append(alert);
			$(".alert").slideDown().delay(3000).slideUp();
		}
	});
}