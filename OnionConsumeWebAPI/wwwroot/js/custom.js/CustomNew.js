function updateCombinedTotal() {
   
    var totalPrice1 = parseFloat($('#AddMeal').text()) || 0;
    var totalPrice2 = parseFloat($('#AddMeal2').text()) || 0;
    var combinedTotal = totalPrice1 + totalPrice2;
    $('#AddMealTotal').text(combinedTotal.toFixed(2));

    var TotalBagAmount = parseFloat($('#AddBaggage').text()) || 0;

    var totalServicePrice1 = parseFloat($('#Addservice').text()) || 0;
    var totalServicePrice2 = parseFloat($('#Addservice2').text()) || 0;
    var combinedServiceTotal = totalServicePrice1 + totalServicePrice2;
    $('#AddServicesTotal').text(combinedServiceTotal.toFixed(2));
    var TotalSeatAmount = parseFloat($('#total').text()) || 0;
    var TotalAmmount = document.getElementById("Totalamount").value;
    var Totalseat1 = parseInt(TotalAmmount) + parseInt(TotalBagAmount) + parseInt(combinedTotal) + parseInt(combinedServiceTotal) + parseInt(TotalSeatAmount);

    if (Totalseat1 != null) {
        document.getElementById("seattotal1").innerHTML = Totalseat1;
    }

}
















//$(document).ready(function () {
//    debugger;
//    var count = $("#passengercountid").val();
//    $('.mycheck').on('change', function () {
//        if ($('.mycheck:checked').length > count) {
//            alert('You can only ' + count + ' select checkboxes.');
//            this.checked = false;
//        }
//    });

//    $('.mycheck2').on('change', function () {
//        if ($('.mycheck2:checked').length > count) {
//            alert('You can only ' + count + ' select checkboxes.');
//            this.checked = false;
//        }
//    });
//    $('input[type="checkbox"]').on('change', function () {
//        var selectedIds = [];
//        debugger;
//        $('input[type="checkbox"]:checked').each(function () {

//            //selectedIds.push($(this).attr('id'));
//            selectedIds.push($(this).val());
//        });
//        var input = document.getElementsByName("unitKey");
//        var total = 0;
//        for (var i = 0; i < input.length; i++) {
//            if (input[i].checked) {
//                total += parseInt(input[i].value);
//            }
//        }
//        document.getElementById("total").value = total.toFixed(2);
//        const seatmap1 = document.getElementById("total").value;

//        var TotalAmmount = document.getElementById("Totalamount").value;
//        localStorage.setItem('seatPayment', seatmap1);
//        localStorage.setItem('basePayment', TotalAmmount);

//        var Totalseat1 = parseInt(TotalAmmount) + parseInt(seatmap1);
//        if (seatmap1 != null) {
//            document.getElementById("seattotal1").innerHTML = Totalseat1;
//        }
//        var TotalSeatPay = parseInt(TotalAmmount) + parseInt(seatmap1) + 100;
//        if (TotalSeatPay != null) {
//            document.getElementById("TotalSeatPayID").innerHTML = TotalSeatPay;
//        }

//        localStorage.setItem("selectedIds", JSON.stringify(selectedIds));


//    });

//    $("#Continuebtn").click(function(){

//        var unitKey = JSON.parse(localStorage.getItem("selectedIds"));
//        $.ajax({
//            url: "/AATripsell/PostUnitkey",
//            type: "POST",
//            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
//            data: { unitKey: unitKey},
//            success: function (data) {
//                window.location.href = '/AirAsiaOneWayPayment/AirAsiaOneWayPaymentView';
//            },
//            error: function (err) {
//                console.error(err);
//            }
//        });
//    });



//});





//////var input = document.getElementsByName("unitKey");


////    window.myfun111= function() {
////        debugger;
////        var input = document.getElementsByName("unitKey");


////    var total = 0;
////    for (var i = 0; i < input.length; i++) {
////    if (input[i].checked) {
////        total += parseInt(input[i].value);
////    }
////        }

////    document.getElementById("total").value =  total.toFixed(2);
////    var one = document.getElementById("Totalamount").value;
////    var two = document.getElementById("total").value;



////        AddMeal();
////    var unitKey = $("#unitKey").val();
////    var passengerkey = $("#passengerkey").val();
////    $.ajax({
////        url: "/AATripsell/PostUnitkey",
////    type: "POST",
////    contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
////    data: {unitKey: unitKey, passengerkey: passengerkey },

////                // data: {"name": $("#txtName").val() },
////    success: function (data) {
////        console.log(data);
////                },

////    error: function (err) {
////        console.error(err);
////                }
////            });
////}



//////*********** Add_Value Total Amount + Meal + Seat ************//
////function AddMeal() {
////    debugger;

////    var seatmap = document.getElementById("total").value;
////    var meal = document.getElementById("inputField").value;
////    document.getElementById("AddMeal").innerHTML = meal;
////    var TotalAmmount = document.getElementById("Totalamount").value;

////    var TotalMeal = parseInt(TotalAmmount) + parseInt(meal);
////    var TotalSeat = parseInt(TotalAmmount) + parseInt(seatmap);
////    var Totalprice1 = parseInt(seatmap) + parseInt(meal) + parseInt(TotalAmmount);

////    if (seatmap != null && meal == "") {
////        document.getElementById("totalservice").innerHTML = TotalSeat;
////    }
////    else if (meal != null && seatmap == "")
////    {
////        document.getElementById("totalMeal").innerHTML = TotalMeal;
////    }
////    else if (Totalprice1 != null){
////        document.getElementById("amount").innerHTML = Totalprice1;
////        document.getElementById("totalservice").style.display = "none";
////        document.getElementById("totalMeal").style.display = "none";

////        var value = Totalprice1;
////        // Store the value in localStorage
////        localStorage.setItem('myValue', value);
////    }

////}



