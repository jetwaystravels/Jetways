

/*********seatmap_tab_select_function_ServiceRequest_PartialView_Page***********/
$(document).ready(function () {
    //$("#tabshow1").hide();
    $("#tabshow2").hide();
    $("#tabshowRT1").hide();
    $("#tabshowRT2").hide();
    $("#tabshow1RT").hide();
    $("#tabshow2RT").hide();

    $("#seattab1way").click(function () {
        $("#tabshow1").show();
        $("#tabshow1RT").hide();
        $("#tabshow2RT").hide();
        $("#tabshowRT1").hide();
        $("#seattab1way").addClass('activeblue');
        $("#seattabRT1way").removeClass('activeblue');
        $("#tabshowRT2").removeClass('activeblue');
        $("#tabshowRT1").removeClass('activeblue');
        $("#seattabRT2").removeClass('activeblue');
        $("#seattabRT1").removeClass('activeblue');
    });
    $("#seattabRT1way").click(function () {
        $("#tabshow1").hide();
        $("#tabshow2").hide();
        $("#tabshow1RT").show();
        $("#tabshow2RT").hide();
        $("#tabshowRT1").show();
        $("#seattabRT1way").addClass('activeblue');
        $("#seattab1way").removeClass('activeblue');
        $("#seattab1").removeClass('activeblue');
        $("#seattab2").removeClass('activeblue');
    });


    $("#seattab1").click(function () {
        $("#tabshow1").show();
        $("#tabshow2").hide();
        $("#tabshow1RT").hide();
        $("#tabshow2RT").hide();
        $("#tabshowRT1").hide();
        $("#tabshowRT2").hide();
        $("#seattab1").addClass('activeblue');
        $("#seattab2").removeClass('activeblue');
        $("#tabshowRT2").removeClass('activeblue');
        $("#tabshowRT1").removeClass('activeblue');
        $("#seattabRT1").removeClass('activeblue');
        $("#seattabRT2").removeClass('activeblue');

    });

    $("#seattab2").click(function () {
        $("#tabshow2").show();
        $("#tabshowRT1").hide();
        $("#tabshowRT2").hide();
        $("#tabshow2RT").hide();
        $("#tabshow1RT").hide();
        $("#seattab2").addClass('activeblue');
        $("#seattab1").removeClass('activeblue');
        $("#seattabRT2").removeClass('activeblue');
        $("#seattabRT1").removeClass('activeblue');
        $("#seattab1way").removeClass('activeblue');
        $("#seattabRT1way").removeClass('activeblue');
        $("#tabshow1").hide();
    });
    $("#seattabRT1").click(function () {
        $("#tabshowRT1").show();
        $("#tabshow2").hide();
        $("#tabshow1").hide();
        $("#tabshow2RT").hide();
        $("#tabshow1RT").show();
        $("#seattabRT1").addClass('activeblue');
        $("#seattabRT2").removeClass('activeblue');
        $("#seattab2").removeClass('activeblue');
        $("#seattab1").removeClass('activeblue');
        $("#seattab1way").removeClass('activeblue');

    });
    $("#seattabRT2").click(function () {
        $("#tabshow2").hide();
        $("#tabshow1").hide();
        $("#tabshow1RT").hide();
        $("#tabshow2RT").show();
        $("#seattabRT2").addClass('activeblue');
        $("#seattabRT1").removeClass('activeblue');
        $("#seattab2").removeClass('activeblue');
        $("#seattab1").removeClass('activeblue');
        $("#seattab1").removeClass('activeblue');
        $("#seattab1way").removeClass('activeblue');
    });



});




function updateCombinedTotalRT() {
    debugger;

    var totalPrice1 = parseFloat($('#AddMeal').text()) || 0;
    var totalPrice2 = parseFloat($('#AddMeal2').text()) || 0;
    var totalPrice1RT = parseFloat($('#AddMealRT').text()) || 0;
    var totalPrice2RT = parseFloat($('#AddMeal2RT').text()) || 0;
    var combinedTotal = totalPrice1 + totalPrice2 + totalPrice1RT + totalPrice2RT;
    $('#AddMealTotalRT').text(combinedTotal.toFixed(2).replace(/\.?0*$/, ''));

    var TotalBagAmount1 = parseFloat($('#AddBaggage').text()) || 0;
    var TotalBagAmountRT = parseFloat($('#AddBaggageRT').text()) || 0;
    var TotalBagAmount = TotalBagAmount1 + TotalBagAmountRT;
    $('#AddBaggageTotalRT').text(TotalBagAmount.toFixed(2).replace(/\.?0*$/, ''));
    //var totalServicePrice1 = parseFloat($('#Addservice').text()) || 0;
    //var totalServicePrice2 = parseFloat($('#Addservice2').text()) || 0;
    //var combinedServiceTotal = totalServicePrice1 + totalServicePrice2;
    //$('#AddServicesTotal').text(combinedServiceTotal.toFixed(2));
    var TotalSeatAmount = parseFloat($('#total').text()) || 0;
    var TotalAmmount = document.getElementById("Totalamount").value;
    var Totalseat1 = parseInt(TotalAmmount) + parseInt(TotalBagAmount) + parseInt(combinedTotal) + parseInt(TotalSeatAmount);

    if (Totalseat1 != null) {
        document.getElementById("seattotal1").innerHTML = Totalseat1;
    }

}



    //Servive Request Code Start







    // service request Code End 