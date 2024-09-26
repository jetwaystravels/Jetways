//**********Validation home page  Code Start ****************//
//*****************************************************//
localStorage.clear();
function validateForm() {
    $('#multipleValuesForm').submit(function (event) {
        var originfocus = document.getElementById("myInput").value;
        if (originfocus == "") {
            //alert("Enter Origin Name");
            var originToDisplay = document.getElementById("errororigin");
            originToDisplay.style.display = "block";
            document.getElementById("myInput").focus();
            return false;
            event.preventDefault(); // Stop the form submission
        }
        var destinationfocus = document.getElementById("myInput1").value;
        if (destinationfocus == "") {
            //alert("Enter Origin Name");
            var originToDisplay = document.getElementById("errordestinaton");
            originToDisplay.style.display = "block";
            document.getElementById("myInput1").focus();
            return false;
        }

        if (originfocus == destinationfocus) {
            alert("Departure and Arrival Airport are same please change it..")
            return false;
            event.preventDefault(); // Stop the form submission

        }

        var startdate = document.getElementById("start-date").value;
        if (startdate == "") {
            //alert("Enter Origin Name");
            var starterror = document.getElementById("startDate");
            starterror.style.display = "block";
            document.getElementById("start-date").focus();
            return false;
            event.preventDefault(); // Stop the form submission
        }

        if ($('#round-tripid').is(':checked')) {
            var endtdate = document.getElementById("end-date").value;
            if (endtdate == "") {
                //alert("Enter Origin Name");
                var enderror = document.getElementById("endDate");
                enderror.style.display = "block";
                document.getElementById("end-date").focus();
                return false;
                event.preventDefault(); // Stop the form submission
            }
        }

        // Traveller Count Validation 9
        var adultCount = parseInt(document.getElementById('field_adult').value);
        var childCount = parseInt(document.getElementById('field_child').value);
        var totalCount = adultCount + childCount;
        if (totalCount > 9) {
            alert("The combined total of adults and children must be 9 or less.");
            return false;
            event.preventDefault();
        }

        //////*****************/////////
        var loader = document.getElementById("loader");
        loader.style.display = "block";
        $("body").css("overflow", "hidden");
        var navtopright = document.getElementById("top-navbar-nav");
        navtopright.style.display = "none";
        var logotop = document.querySelector(".navbar-brand");
        logotop.style.display = "none";
    });

    //var selectedClass = document.querySelector('input[name="flightclass"]:checked').value;
    //alert("1"+selectedClass);
    //localStorage.setItem('selectedFlightClass', selectedClass);
    //var selectedClass = localStorage.getItem('selectedFlightClass');
    //alert("2"+selectedClass);
    //if (selectedClass) {
    //    document.querySelector('.flightClassName').textContent = selectedClass;
    //}
};

function checkOnlyThis(checkbox) {
    var checkboxes = document.getElementsByName('farecheck');
    checkboxes.forEach((item) => {
        if (item !== checkbox) {
            item.checked = false;
        }
    });
}

//**********Chosen Arrival Code Start ****************//
//*****************************************************//
$(document).ready(function () {
    $('#arrivalItemId').chosen();
    $('#arrivalItemId').trigger('chosen:open');


    $('#basic-addon1').on('click', function (event) {
        event.stopPropagation();
        $('#arrivalItemId').trigger('chosen:open');
        $('.chosen-drop').show();
        $('.autoarrival input').attr('placeholder', 'To');
        $('.autoarrival').show();
        $('.autodropdown').hide();
        var chosenInput2 = document.querySelector('.autoarrival input');
        chosenInput2.focus();
        chosenInput2.addEventListener("keydown", function (event) {
            if (event.key === " ") {
                event.preventDefault();
                alert("Whitespace is not allowed");
            }
        });
        chosenInput2.addEventListener("paste", function (event) {
            event.preventDefault();
        });

    });

    $(document.body).on('click', function (event) {
        if (!$(event.target).closest('.chosen-drop').length && !$(event.target).is('#myInput1')) {
            $('.chosen-drop').hide();
        }
    });


});



document.addEventListener('DOMContentLoaded', function () {


    var input = document.getElementById("myInput1");
    input.addEventListener("keydown", function (event) {
        event.preventDefault();
    });

    // Disable pasting
    input.addEventListener("paste", function (event) {
        event.preventDefault();
    });
    var inputdate1 = document.getElementById("start-date");
    inputdate1.addEventListener("keydown", function (event) {
        event.preventDefault();
    });

    // Disable pasting
    inputdate1.addEventListener("paste", function (event) {
        event.preventDefault();
    });
    var inputdate2 = document.getElementById("end-date");
    inputdate2.addEventListener("keydown", function (event) {
        event.preventDefault();
    });

    // Disable pasting
    inputdate2.addEventListener("paste", function (event) {
        event.preventDefault();
    });

    $('.chosen-drop').css('display', 'none');
    var CityName = "Mumbai";
    var CityCode = "BOM";
    var AirportName = "Chhatrapati Shivaji Maharaj International Airport";
    document.getElementById("myInput1").value = CityName + "-" + CityCode;
    document.getElementById("airportArval").innerHTML = AirportName;
    var airportElement = document.getElementById("airportArval");
    airportElement.innerHTML = AirportName;
    airportElement.title = AirportName;

});

function toggleDropdownArrival() {
    //$('.chosen-drop').toggle();
    var chosenInputA = document.querySelector('.autoarrival input');
    chosenInputA.focus();
    if ($('.chosen-drop').is(':visible')) {
        $('.autoarrival input').focus();
        $('.chosen-drop').show();
    }
}


function arrivalSelection() {
    $('.autoarrival input').focus();
    //alert("Hello Test");
    var dropdown = document.getElementById("arrivalItemId");
    var selectedValue = dropdown.value;
    var selectedOption = dropdown.options[dropdown.selectedIndex];
    var cityDetails = selectedOption.text.split('-');
    var cityName = cityDetails[0].trim();
    var cityCode = cityDetails[1].trim();
    var airportName = cityDetails[2].trim();

    //alert("Selected City Name: " + cityName);
    //alert("Selected City Code: " + cityCode);
    //alert("Selected Airport Name: " + airportName);
    document.getElementById("myInput1").value = cityName + "-" + cityCode;
    document.getElementById("airportArval").innerHTML = airportName;
    var airportElement = document.getElementById("airportArval");
    airportElement.innerHTML = airportName;
    airportElement.title = airportName;
    $('.chosen-drop').hide();
    dropdown.selectedIndex = -1;
}



//**********Chosen Departure Code Start ****************//
//*****************************************************//
$(document).ready(function () {
    $('#selectedItemId').chosen();
    $('.chosen-drop').hide();
    $('#selectedItemId').trigger('chosen:open');

    $('#myInputbx').on('click', function (event) {
        event.stopPropagation();
        $('#selectedItemId').trigger('chosen:open');
        $('.chosen-drop').show();
        $('.chosen-search input').attr('placeholder', ' From');
        $('.autoarrival').hide();
        $('.autodropdown').show();
        var chosenInput = document.querySelector('.chosen-search input');
        chosenInput.focus();
        chosenInput.addEventListener("keydown", function (event) {
            if (event.key === " ") {
                event.preventDefault(); 
                alert("Whitespace is not allowed");
            }
        });
        chosenInput.addEventListener("paste", function (event) {
            event.preventDefault();
        });
    });


    $(document.body).on('click', function (event) {
        if (!$(event.target).closest('.chosen-drop').length && !$(event.target).is('#myInput')) {
            $('.chosen-drop').hide();
        }
    });
});



document.addEventListener('DOMContentLoaded', function () {
    $('.chosen-drop').css('display', 'none');
    var defaultCityName = "New Delhi";
    var defaultCityCode = "DEL";
    var defaultAirportName = "Indira Gandhi International Airport";
    document.getElementById("myInput").value = defaultCityName + "-" + defaultCityCode;
    document.getElementById("airportName").innerHTML = defaultAirportName;
    var airportElement = document.getElementById("airportName");
    airportElement.innerHTML = defaultAirportName;
    airportElement.title = defaultAirportName;

    var inputdep = document.getElementById("myInput");
    inputdep.addEventListener("keydown", function (event) {
        event.preventDefault();
    });

    // Disable pasting
    inputdep.addEventListener("paste", function (event) {
        event.preventDefault();
    });

});


function toggleDropdown() {
    //$('.chosen-drop').toggle();
    var chosenInput = document.querySelector('.chosen-search input');
    chosenInput.focus();
    if ($('.chosen-drop').is(':visible')) {
        $('.chosen-search input').focus();
        $('.chosen-drop').show();
    }
}

function handleSelection() {
    $('.chosen-search input').focus();
    //alert("Hello Test");
    var dropdown = document.getElementById("selectedItemId");
    var selectedValue = dropdown.value;
    var selectedOption = dropdown.options[dropdown.selectedIndex];
    var cityDetails = selectedOption.text.split('-');
    var cityName = cityDetails[0].trim();
    var cityCode = cityDetails[1].trim();
    var airportName = cityDetails[2].trim();

    //alert("Selected City Name: " + cityName);
    //alert("Selected City Code: " + cityCode);
    //alert("Selected Airport Name: " + airportName);
    document.getElementById("myInput").value = cityName + "-" + cityCode;
    document.getElementById("airportName").innerHTML = airportName;
    var airportElement = document.getElementById("airportName");
    airportElement.innerHTML = airportName;
    airportElement.title = airportName;
    $('.chosen-drop').hide();
    dropdown.selectedIndex = -1;
}
//***********From Cityname DropDown end********************//






////*****-----------RoundTrip end date disabled--********------//
$(document).ready(function () {

    //****  Replace Icon JS Start*/
    $('.rplc-btn').on('click', function (event) {
        var flyingFromValue = $('#myInput').val();
        var flyingToValue = $('#myInput1').val();
        //var airportElement = document.getElementById("airportName");
        //var airportName = airportElement.textContent;
        //var airportElement2 = document.getElementById("airportArval");
        //var airportName2 = airportElement2.textContent;

        // Swap input field values
        $('#myInput').val(flyingToValue);
        $('#myInput1').val(flyingFromValue);

        // Swap text content of airport elements
        var airportName = $('#airportName').text();
        var airportName2 = $('#airportArval').text();
        $('#airportName').text(airportName2).attr('title', airportName2);
        $('#airportArval').text(airportName).attr('title', airportName);



        // Check if departure and arrival airports are the same
        if (flyingFromValue == flyingToValue || airportName == airportName2) {
            alert("Departure and Arrival Airport are the same. Please change it.");
            return false;
            event.preventDefault();
        }
    });

    //**********Replace Icon JS End**********/
    $('.S-option input[type="radio"]').on('change', function () {
        // Uncheck all radio buttons in the same group except the currently selected one
        $('.S-option input[type="radio"]').not(this).prop('checked', false);

    });

    $('#end-date').prop('disabled', true);
    $('#bgEnddate').css('background-color', '#e9ecef');


    // Event handler for radio button change
    $('input[type="radio"]').on('change', function () {
        if ($('#round-tripid').is(':checked')) {

            $('#end-date').prop('disabled', false);
            $('#bgEnddate').css('background-color', '#fff');
            $('#end-date').css('visibility', 'visible');
            //Date Picker end date

            //var returndate = new Date();
            //var returndd = String(returndate.getDate() + 2).padStart(2, '0');
            //var returnmm = String(returndate.getMonth() + 1).padStart(2, '0');
            //var returnyyyy = returndate.getFullYear();
            //var returncurrentDate = returnyyyy + '-' + returnmm + '-' + returndd;
            //$("#end-date").val(returncurrentDate);

            //$("#end-date").datepicker(
            //    {
            //        dateFormat: 'yy-mm-dd',
            //        numberOfMonths: 2,
            //        maxDate: '+3m',
            //        minDate: '0'
            //    });

            //$("#start-date").val(returncurrentDate);

            var returndate = new Date();
            returndate.setDate(returndate.getDate() + 2); // Add 2 days to the current date
            debugger;
            var returndd = returndate.getDate(); // Note: Don't pad with 0
            var returnmm = returndate.getMonth() + 1; // Note: Don't pad with 0
            var returnyyyy = returndate.getFullYear();

            // Check if the return day exceeds the maximum number of days in the month
            if (returndd > new Date(returnyyyy, returnmm, 0).getDate()) {
                returndd = new Date(returnyyyy, returnmm, 0).getDate(); // Set to the last day of the month
            }

            // Format the date to yyyy-mm-dd, ensuring each part is padded with a leading zero if necessary
            var returncurrentDate = returnyyyy + '-' + (returnmm < 10 ? '0' + returnmm : returnmm) + '-' + (returndd < 10 ? '0' + returndd : returndd);

            var startdate = new Date();
            startdate.setDate(startdate.getDate() + 1);
            var startdd = startdate.getDate(); // Note: Don't pad with 0
            var startnmm = startdate.getMonth() + 1; // Note: Don't pad with 0
            var startyyyy = startdate.getFullYear();

            // Check if the return day exceeds the maximum number of days in the month
            if (startdd > new Date(startyyyy, startnmm, 0).getDate()) {
                startdd = new Date(startyyyy, startnmm, 0).getDate(); // Set to the last day of the month
            }

            // Format the date to yyyy-mm-dd, ensuring each part is padded with a leading zero if necessary
            var startcurrentDate = startyyyy + '-' + (startnmm < 10 ? '0' + startnmm : startnmm) + '-' + (startdd < 10 ? '0' + startdd : startdd);

            $("#end-date").val(returncurrentDate);

            $("#end-date").datepicker({
                dateFormat: 'yy-mm-dd',
                numberOfMonths: 2,
                maxDate: '+3m',
                minDate: '0'
            });

            $("#start-date").val(startcurrentDate);
            $("#start-date").datepicker(
                {
                    dateFormat: 'yy-mm-dd',
                    numberOfMonths: 2,
                    maxDate: '+2m',
                    minDate: '0',
                    onSelect: function (selectedDate) {
                        var endDate = $('#end-date');
                        endDate.datepicker('option', 'minDate', selectedDate);
                        endDate.datepicker('setDate', selectedDate);

                    }
                });

            const elementToHide = document.querySelector('.rounddateinput');
            elementToHide.style.display = 'none'; // Hide the element


            ///Date picker End Date---End--
        }

        else if ($('#multi-cityid').is(':checked')) {

            $('#end-date').prop('disabled', false);
            $('#bgEnddate').css('background-color', '#fff');
            $('#end-date').css('visibility', 'visible');
            //Date Picker end date

            //var returndate = new Date();
            //var returndd = String(returndate.getDate() + 2).padStart(2, '0');
            //var returnmm = String(returndate.getMonth() + 1).padStart(2, '0');
            //var returnyyyy = returndate.getFullYear();
            //var returncurrentDate = returnyyyy + '-' + returnmm + '-' + returndd;
            //$("#end-date").val(returncurrentDate);

            //$("#end-date").datepicker(
            //    {
            //        dateFormat: 'yy-mm-dd',
            //        numberOfMonths: 2,
            //        maxDate: '+3m',
            //        minDate: '0'
            //    });

            //$("#start-date").val(returncurrentDate);

            var returndate = new Date();
            returndate.setDate(returndate.getDate() + 2); // Add 2 days to the current date
            debugger;
            var returndd = returndate.getDate(); // Note: Don't pad with 0
            var returnmm = returndate.getMonth() + 1; // Note: Don't pad with 0
            var returnyyyy = returndate.getFullYear();

            // Check if the return day exceeds the maximum number of days in the month
            if (returndd > new Date(returnyyyy, returnmm, 0).getDate()) {
                returndd = new Date(returnyyyy, returnmm, 0).getDate(); // Set to the last day of the month
            }

            // Format the date to yyyy-mm-dd, ensuring each part is padded with a leading zero if necessary
            var returncurrentDate = returnyyyy + '-' + (returnmm < 10 ? '0' + returnmm : returnmm) + '-' + (returndd < 10 ? '0' + returndd : returndd);

            var startdate = new Date();
            startdate.setDate(startdate.getDate() + 1);
            var startdd = startdate.getDate(); // Note: Don't pad with 0
            var startnmm = startdate.getMonth() + 1; // Note: Don't pad with 0
            var startyyyy = startdate.getFullYear();

            // Check if the return day exceeds the maximum number of days in the month
            if (startdd > new Date(startyyyy, startnmm, 0).getDate()) {
                startdd = new Date(startyyyy, startnmm, 0).getDate(); // Set to the last day of the month
            }

            // Format the date to yyyy-mm-dd, ensuring each part is padded with a leading zero if necessary
            var startcurrentDate = startyyyy + '-' + (startnmm < 10 ? '0' + startnmm : startnmm) + '-' + (startdd < 10 ? '0' + startdd : startdd);

            $("#end-date").val(returncurrentDate);

            $("#end-date").datepicker({
                dateFormat: 'yy-mm-dd',
                numberOfMonths: 2,
                maxDate: '+3m',
                minDate: '0'
            });

            $("#start-date").val(startcurrentDate);
            $("#start-date").datepicker(
                {
                    dateFormat: 'yy-mm-dd',
                    numberOfMonths: 2,
                    maxDate: '+2m',
                    minDate: '0',
                    onSelect: function (selectedDate) {
                        var endDate = $('#end-date');
                        endDate.datepicker('option', 'minDate', selectedDate);
                        endDate.datepicker('setDate', selectedDate);

                    }
                });

            const elementToHide = document.querySelector('.rounddateinput');
            elementToHide.style.display = 'none'; // Hide the element


            ///Date picker End Date---End--
        }




        else {
            // Disable the end date input field for other options
            $('#end-date').prop('disabled', true);
            $('#bgEnddate').css('background-color', '#e9ecef');
            $('#end-date').css('visibility', 'hidden');
            const elementToHide = document.querySelector('.rounddateinput');
            elementToHide.style.display = 'block'; // Hide the element

        }

    });

    //$('input[type="radio"]').on('change', function () {
    //    if ($('#round-tripid').is(':checked')) {

    //        $('#end-date').prop('disabled', false);
    //        $('#bgEnddate').css('background-color', '#fff');
    //        $('#end-date').css('visibility', 'visible');

    //        // Date Picker end date
    //        var returndate = new Date();
    //        var returndd = returndate.getDate() + 2; // Note: Don't pad with 0
    //        var returnmm = returndate.getMonth() + 1; // Note: Don't pad with 0
    //        var returnyyyy = returndate.getFullYear();

    //        // Check if the return day exceeds the maximum number of days in the month
    //        if (returndd > new Date(returnyyyy, returnmm, 0).getDate()) {
    //            returndd = new Date(returnyyyy, returnmm, 0).getDate(); // Set to the last day of the month
    //        }

    //        var returncurrentDate = returnyyyy + '-' + returnmm + '-' + returndd;
    //        $("#end-date").val(returncurrentDate);

    //        $("#end-date").datepicker({
    //            dateFormat: 'yy-mm-dd',
    //            numberOfMonths: 2,
    //            maxDate: '+3m',
    //            minDate: '0'
    //        });

    //        $("#start-date").val(returncurrentDate);
    //        $("#start-date").datepicker({
    //            dateFormat: 'yy-mm-dd',
    //            numberOfMonths: 2,
    //            maxDate: '+2m',
    //            minDate: '0',
    //            onSelect: function (selectedDate) {
    //                var endDate = $('#end-date');
    //                endDate.datepicker('option', 'minDate', selectedDate);
    //                endDate.datepicker('setDate', selectedDate);
    //                var maxEndDate = new Date(selectedDate);
    //                maxEndDate.setMonth(maxEndDate.getMonth() + 3); 
    //                endDate.datepicker('option', 'maxDate', maxEndDate);

    //                if (endDate.val() < selectedDate) {
    //                    endDate.datepicker('setDate', selectedDate);
    //                }
    //            }
    //        });

    //        const elementToHide = document.querySelector('.rounddateinput');
    //        elementToHide.style.display = 'none'; // Hide the element

    //    } else {
    //        // Disable the end date input field for other options
    //        $('#end-date').prop('disabled', true);
    //        $('#bgEnddate').css('background-color', '#e9ecef');
    //        $('#end-date').css('visibility', 'hidden');
    //        const elementToHide = document.querySelector('.rounddateinput');
    //        elementToHide.style.display = 'block'; // Hide the element

    //    }
    //});








    ////*****-----------DatePicker Start*****-----------//
    //$(function () {
    //    var today = new Date();
    //    var dd = String(today.getDate() + 1).padStart(2, '0');
    //    var mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
    //    var yyyy = today.getFullYear();
    //    var currentDate = yyyy + '-' + mm + '-' + dd;

    //    // Set the current date as the default value in the input field
    //    $("#start-date").val(currentDate);
    //    $("#start-date").datepicker( {
    //            dateFormat: 'yy-mm-dd',
    //            numberOfMonths: 2,
    //            maxDate: '+2m',
    //            minDate: '0',
    //           onSelect: function (selectedDate) {
    //            var endDate = $('#end-date');
    //            endDate.datepicker('option', 'minDate', selectedDate);
    //            endDate.datepicker('setDate', selectedDate);
    //           }
    //    });


    //});

    $(function () {
        var today = new Date();
        var dd = String(today.getDate()).padStart(2, '0');
        var mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
        var yyyy = today.getFullYear();
        var maxDays = new Date(yyyy, mm, 0).getDate(); // Get the maximum days for the current month
        var currentDate = yyyy + '-' + mm + '-' + dd;

        // Set the current date as the default value in the input field
        $("#start-date").val(currentDate);
        $("#start-date").datepicker({
            dateFormat: 'yy-mm-dd',
            numberOfMonths: 2,
            maxDate: '+' + maxDays + 'd', // Set the maximum date to the last day of the current month
            minDate: '0',
            onSelect: function (selectedDate) {
                var endDate = $('#end-date');
                endDate.datepicker('option', 'minDate', selectedDate);
                endDate.datepicker('setDate', selectedDate);
            }
        });
    });





    var maxField = 6;
    var maxField_adult = 9;
    var count_adult = parseInt(localStorage.getItem("adultcount")) || 1; // Retrieve adult count from local storage or default to 1
    var count_child = 0;
    var count_infant = 0;

    $('#count_adult').text(count_adult);
    $('#field_adult').val(count_adult);

    $('.increment').on('click', function () {
        if (count_adult < maxField_adult) {
            count_adult++;
            $('#count_adult').text(count_adult);
            $('#field_adult').val(count_adult);
            localStorage.setItem("adultcount", count_adult); // Store adult count in local storage
        }
    });

    $('.decrement').on('click', function () {
        if (count_adult > 1) {
            count_adult--;
            $('#count_adult').text(count_adult);
            $('#field_adult').val(count_adult);
            localStorage.setItem("adultcount", count_adult); // Store adult count in local storage

            // Adjust infant count to match adult count
            if (count_infant > count_adult) {
                count_infant = count_adult;
                $('#count_infant').text(count_infant);
                $('#field_infant').val(count_infant);
            }
        }
    });

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

    $('.increment2').on('click', function () {
        if (count_infant < maxField) {
            var adultcounttotal = parseInt(localStorage.getItem("adultcount")) || 1; // Retrieve adult count from local storage or default to 1
            if (count_infant >= adultcounttotal) {
                alert("Number of infants cannot be more than adults");
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


