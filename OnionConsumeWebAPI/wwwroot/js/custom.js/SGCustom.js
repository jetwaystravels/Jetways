var selectedSSRKeys = [];

function selectSSR(button) {
    debugger;
    var ssrKeyAttribute = button.getAttribute('data-ssrkey');

    var ssrKeydata = document.getElementById(ssrKeyAttribute).value;
    var wordsArray = ssrKeydata.split(' ');
    alert(wordsArray);
    var meal = null;
    var ssrKey = null;
    if (wordsArray.length > 1) {
        ssrKey = wordsArray[0];
        var ssrKey1 = ssrKey.split('"');
        var sskeydata = ssrKey1[1];
        ssrKey = sskeydata;
        alert(ssrKey);
        meal = wordsArray[1];
    }

    // Check if the ssrKey is already in the selectedSSRKeys array
    var index = selectedSSRKeys.indexOf(ssrKey + "-" + meal);


    if (index === -1) {
        // If not in the array, add it
        selectedSSRKeys.push(ssrKey + "-" + meal);
        button.classList.add('selected'); // Add a class to indicate selection
    } else {
        // If already in the array, remove it
        selectedSSRKeys.splice(index, 1);
        button.classList.remove('selected'); // Remove the selection class
    }

    // Print the selectedSSRKeys array to the console for testing
    console.log(selectedSSRKeys);

    if (selectedSSRKeys.length >= 1) {
        AddSSRMeal();
    }
    
}


function AddSSRMeal() {
    debugger;
    var meal = 0;
    var keyssr = [];
    for (var i = 0; i < selectedSSRKeys.length; i++)
    {
        debugger;

        var keymeal = null;
        //alert(selectedSSRKeys[i]);
        var wordsArray = selectedSSRKeys[i].split('-');
        //alert(wordsArray);
        if (wordsArray.length > 1) {
            ssrKey = wordsArray[0];
            //alert(ssrKey);
            //alert(wordsArray[1].replace('"', ''));
            meal += parseInt(wordsArray[1].replace('"', ''));
            keyssr.push(ssrKey);
        }
        //meal += parseInt(selectedSSRKeys[i].value);

    }

    //here
    var ssrkeylist = keyssr;
    // till here
    var seatmap = document.getElementById("total").value;
    if (ssrKey != "") {
        document.getElementById("AddMeal").innerHTML = meal;
    }
    var TotalAmmount = document.getElementById("Totalamount").value;

    var TotalMeal = parseInt(TotalAmmount) + parseInt(meal);
    var TotalSeat = parseInt(TotalAmmount) + parseInt(seatmap);

    //alert("1" + seatmap + "2" + meal + "3" + TotalAmmount);
    var Totalprice1 = parseInt(seatmap) + parseInt(meal) + parseInt(TotalAmmount);

    if (seatmap != null && meal == "") {
        document.getElementById("totalservice").innerHTML = TotalSeat;
    }
    else if (meal != null && seatmap == "") {
        document.getElementById("totalMeal").innerHTML = TotalMeal;
    }
    else if (Totalprice1 != null) {
        document.getElementById("amount").innerHTML = Totalprice1;
        document.getElementById("totalservice").style.display = "none";
        document.getElementById("totalMeal").style.display = "none";

        var value = Totalprice1;
        // Store the value in localStorage
        localStorage.setItem('myValue', value);
    }
    var selectedIds = localStorage.getItem('selectedIds');

    //alert(ssrKey + " " + selectedIds);

    //$.ajax({
    //    url: "/SGTripsell/PostUnitkey",
    //    type: "POST",
    //    contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
    //    data: { selectedIds: selectedIds, ssrKey: ssrkeylist },
    //    success: function (data) {
    //        console.log(data);
    //    },
    //    error: function (err) {
    //        console.error(err);
    //    }
    //});

}
//*********** Add_Value Total Amount + Meal + Seat ************//
function SSRMeal() {
    debugger;

    var ssrKeyAttribute = button.getAttribute("data-ssrkey");

    // Use the value of the data-ssrkey attribute to find the related hidden input

    var ssrKeydata = document.getElementById(ssrKeyAttribute).value;
    var wordsArray = ssrKeydata.split(' ');
    //alert(wordsArray);
    var meal = null;
    var ssrKey = null;
    if (wordsArray.length > 1) {
        ssrKey = wordsArray[0];
        var ssrKey1 = ssrKey.split('"');
        var sskeydata = ssrKey1[1];
        ssrKey = sskeydata;
        //alert(ssrKey);
        meal = wordsArray[1];
    }
    var seatmap = document.getElementById("total").value;
    if (ssrKey != "") {
        //alert("Mealtest " + document.getElementById("inputField").value)
        //meal = document.getElementById("inputField").value;
        document.getElementById("AddMeal").innerHTML = meal;

    }
    //else {
    //    alert("elseno")
    //}

    var TotalAmmount = document.getElementById("Totalamount").value;

    var TotalMeal = parseInt(TotalAmmount) + parseInt(meal);
    var TotalSeat = parseInt(TotalAmmount) + parseInt(seatmap);

    //alert("1" + seatmap + "2" + meal + "3" + TotalAmmount);
    var Totalprice1 = parseInt(seatmap) + parseInt(meal) + parseInt(TotalAmmount);

    if (seatmap != null && meal == "") {
        document.getElementById("totalservice").innerHTML = TotalSeat;
    }
    else if (meal != null && seatmap == "") {
        document.getElementById("totalMeal").innerHTML = TotalMeal;
    }
    else if (Totalprice1 != null) {
        document.getElementById("amount").innerHTML = Totalprice1;
        document.getElementById("totalservice").style.display = "none";
        document.getElementById("totalMeal").style.display = "none";

        var value = Totalprice1;
        // Store the value in localStorage
        localStorage.setItem('myValue', value);
    }
    var selectedIds = localStorage.getItem('selectedIds');

    //alert(ssrKey + " " + selectedIds);
   
    $.ajax({
        url: "/SGTripsell/PostUnitkey",
        type: "POST",
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: { selectedIds: selectedIds, ssrKey: ssrKey },
        success: function (data) {
            console.log(data);
        },
        error: function (err) {
            console.error(err);
        }
    });
}



