function updateCombinedTotal() {
    var totalPrice1 = parseFloat($('#AddMeal').text()) || 0;
    var totalPrice2 = parseFloat($('#AddMeal2').text()) || 0;
    var combinedTotal = totalPrice1 + totalPrice2;
    $('#AddMealTotal').text(combinedTotal.toFixed(2).replace(/\.?0*$/, ''));

    var TotalBagAmount1 = parseFloat($('#AddBaggage').text()) || 0;
    var fastForward = parseFloat($('#priceDisplay').text()) || 0;
    var TotalSeatAmount = parseFloat($('#total').text()) || 0;
    var TotalAmmount = document.getElementById("Totalamount").value;
    var Totalseat1 = parseInt(TotalAmmount) + parseInt(TotalBagAmount1) + parseInt(combinedTotal) + parseInt(TotalSeatAmount) + parseInt(fastForward);

    if (Totalseat1 != null) {
        document.getElementById("seattotal1").innerHTML = Totalseat1;
    }

}