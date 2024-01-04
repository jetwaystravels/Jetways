function validateForm() {
    var originfocus = document.getElementById("airportInput").value;
    if (originfocus == "") {
        //alert("Enter Origin Name");
        var originToDisplay = document.getElementById("errororigin");
        originToDisplay.style.display = "block";
        document.getElementById("airportInput").focus();
        return false;
    }
    var destinationfocus = document.getElementById("airportdestinationInput").value;
    if (destinationfocus == "") {
        //alert("Enter Origin Name");
        var originToDisplay = document.getElementById("errordestinaton");
        originToDisplay.style.display = "block";
        document.getElementById("airportdestinationInput").focus();
        return false;
    }
    //var sdate = document.getElementById("start-date").value;
    //if (sdate == "") {
    //    //alert("Enter Start Date");
    //    var originToDisplay = document.getElementById("errorbegindate");
    //    originToDisplay.style.display = "block";
    //    document.getElementById("start-date").focus();
    //    return false;

    //}


    //var radioButton = document.getElementById("round-tripid"); // Replace "option1" with your radio button's ID

    //if (radioButton.checked) {
    //    var enddate = document.getElementById("end-date").value;
    //    if (enddate == "") {
    //        alert("Enter End Date");
    //        return false;
    //    }
    //}



}






////*****-----------RoundTrip end date disabled--********------//
$(document).ready(function () {
    $('.S-option input[type="radio"]').on('change', function () {
        // Uncheck all radio buttons in the same group except the currently selected one
        $('.S-option input[type="radio"]').not(this).prop('checked', false);
    });
    $('#end-date').prop('disabled', true);

    // Event handler for radio button change
    $('input[type="radio"]').on('change', function () {
        if ($('#round-tripid').is(':checked')) {

            $('#end-date').prop('disabled', false);
            //Date Picker end date
            var returndate = new Date();
            var returndd = String(returndate.getDate() + 2).padStart(2, '0');
            var returnmm = String(returndate.getMonth() + 1).padStart(2, '0');
            var returnyyyy = returndate.getFullYear();
            var returncurrentDate = returnyyyy + '-' + returnmm + '-' + returndd;
            $("#end-date").val(returncurrentDate);

            $("#end-date").datepicker(
                {
                    dateFormat: 'yy-mm-dd',
                    numberOfMonths: 2,
                    maxDate: '+3m',
                    minDate: '0'
                });

            const elementToHide = document.querySelector('.rounddateinput');
            elementToHide.style.display = 'none'; // Hide the element


            ///Date picker End Date---End--
        } else {
            // Disable the end date input field for other options
            $('#end-date').prop('disabled', true);
        }

    });

    ////*****-----------DatePicker Start*****-----------//
    $(function () {
        var today = new Date();
        var dd = String(today.getDate() + 1).padStart(2, '0');
        var mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
        var yyyy = today.getFullYear();
        var currentDate = yyyy + '-' + mm + '-' + dd;

        // Set the current date as the default value in the input field
        $("#start-date").val(currentDate);
        $("#start-date").datepicker(
            {
                dateFormat: 'yy-mm-dd',
                numberOfMonths: 2,
                maxDate: '+2m',
                minDate: '0'
            });


    });


    //*****-----------Travell Detail , Adult Count, Child Count, Infant Count!!----*******//

    var maxField = 6;
    var maxField_adult = 9;
    var count = 1;
    $('.increment').on('click', function () {
        if (count < maxField_adult) {
            count++;
            $('#count_adult').text(count);
            $('#field_adult').val(count);

        }

        var fieldValue = $('#field_adult').val();
        localStorage.setItem("adultcount", fieldValue);



    });

    $('.decrement').on('click', function () {
        if (count > 0) {
            count--;
            $('#count_adult').text(count);
            $('#field_adult').val(count);

        }

    });

    var count_child = 0;
    $('.increment1').on('click', function () {
        if (count_child < maxField) {
            count_child++;
            $('#count_child').text(count_child);
            $('#field_child').val(count_child);
        }
    });

    $('.decrement1').on('click', function () {
        if (count_child > 0) {
            count_child--;
            $('#count_child').text(count_child);
            $('#field_child').val(count_child);
        }
    });

    var count_infant = 0;
    $('.increment2').on('click', function () {
        if (count_infant < maxField) {
            var adultcounttotal = localStorage.getItem("adultcount");
            if (adultcounttotal == null) {
                //alert("abc");
                adultcounttotal = 1;
                //alert(adultcounttotal);
            }



            //alert(count_infant);
            if (adultcounttotal <= count_infant) {
                //alert(count_infant);
                alert("Number of infants cannot be more than adults");
                document.getElementById("field_adult").focus();
                return false;
            }

            count_infant++;
            $('#count_infant').text(count_infant);
            $('#field_infant').val(count_infant);


        }

    });

    $('.decrement2').on('click', function () {
        if (count_infant > 0) {
            count_infant--;
            $('#count_infant').text(count_infant);
            $('#field_infant').val(count_infant);
        }
    });


});









//*****-----------Auto Suggest City Name , Airport Name and Airport Code----********----------//
const airportInput = document.getElementById('airportInput');
const airportSuggestions = document.getElementById('airportSuggestions');

// Function to filter and display suggestions
function showSuggestions(event) {
    const userInput = event.target.value.toLowerCase();
    airportSuggestions.innerHTML = '';

    const filteredAirports = airportData.filter(airport => {
        return (
            airport.city.toLowerCase().includes(userInput) ||
            airport.code.toLowerCase().includes(userInput) ||
            airport.name.toLowerCase().includes(userInput)
        );
    });

    filteredAirports.forEach(airport => {
        const li = document.createElement('li');
        //li.textContent = `${airport.city} -  ${airport.code},${airport.name}`;
        const cityCodeText = document.createTextNode(`${airport.city} -  ${airport.code},`);
        const nameText = document.createTextNode(airport.name);

        li.appendChild(cityCodeText);
        li.appendChild(document.createElement('br')); // Line break between city/code and name
        li.appendChild(nameText);

        li.addEventListener('click', () => {
            airportInput.value = `${airport.city} - ${airport.code}`;
            airportSuggestions.innerHTML = '';
            document.getElementById("airportname").innerHTML = nameText.textContent;

        });

        airportSuggestions.appendChild(li);

    });
}

// Attach event listener to input field for showing suggestions
airportInput.addEventListener('input', showSuggestions);
document.addEventListener('click', function (event) {

    const airportSuggestions = document.getElementById('airportSuggestions');
    const airportInput = document.getElementById('airportInput');
    const airportName = document.getElementById('airportname');

    // Check if the clicked target is not within airportInput or airportSuggestions
    if (!airportInput.contains(event.target) && !airportSuggestions.contains(event.target)) {
        airportSuggestions.style.display = 'none';
        //airportName.style.display = 'none';
        // Additional logic or actions to perform when hiding the suggestions and name
    } else {
        airportSuggestions.style.display = 'block';
        airportName.style.display = 'block';
        var originToDisplay = document.getElementById("errororigin");
        originToDisplay.style.display = "none";


    }

});



//*****-----------Auto Suggest City Name , Airport Name and Airport Code----********----------//
const airportdestinationInput = document.getElementById('airportdestinationInput');
const airportSuggestionsDestination = document.getElementById('airportSuggestionsDestination');
// Function to filter and display suggestions
function showSuggestionsData(event) {
    const userInput = event.target.value.toLowerCase();
    airportSuggestionsDestination.innerHTML = '';

    const filteredAirports = airportData.filter(airport => {
        return (

            airport.city.toLowerCase().includes(userInput) ||
            airport.code.toLowerCase().includes(userInput) ||
            airport.name.toLowerCase().includes(userInput)
        );
    });

    filteredAirports.forEach(airport => {
        const li = document.createElement('li');
        //li.textContent = `${airport.city} -  ${airport.code},${airport.name}`;
        const cityCodeText = document.createTextNode(`${airport.city} -  ${airport.code},`);
        const nameText = document.createTextNode(airport.name);

        li.appendChild(cityCodeText);
        li.appendChild(document.createElement('br')); // Line break between city/code and name
        li.appendChild(nameText);

        li.addEventListener('click', () => {
            airportdestinationInput.value = `${airport.city} - ${airport.code}`;
            airportSuggestionsDestination.innerHTML = '';
            document.getElementById("airportdestination").innerHTML = nameText.textContent;

        });

        airportSuggestionsDestination.appendChild(li);

    });
}

// Attach event listener to input field for showing suggestions
airportdestinationInput.addEventListener('input', showSuggestionsData);
document.addEventListener('click', function (event) {

    const airportSuggestionsDestination = document.getElementById('airportSuggestionsDestination');
    const airportdestinationInput = document.getElementById('airportdestinationInput');
    const airportName = document.getElementById('airportname');

    // Check if the clicked target is not within airportInput or airportSuggestions
    if (!airportdestinationInput.contains(event.target) && !airportSuggestionsDestination.contains(event.target)) {
        airportSuggestionsDestination.style.display = 'none';
        //airportName.style.display = 'none';
        // Additional logic or actions to perform when hiding the suggestions and name
    } else {
        airportSuggestionsDestination.style.display = 'block';
        airportName.style.display = 'block';
        var destiToDisplay = document.getElementById("errordestinaton");
        destiToDisplay.style.display = "none";
    }
});



