//**********Validation home page  Code Start ****************//
//*****************************************************//
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
    });
};



//**********Chosen Arrival Code Start ****************//
//*****************************************************//
$(document).ready(function () {
    $('#arrivalItemId').chosen();
    $('#arrivalItemId').trigger('chosen:open');
    $('#myInput1').on('click', function (event) {
        event.stopPropagation();
        $('.chosen-drop').show();
        $('.autoarrival input').attr('placeholder', 'To');
        $('.autoarrival').show();
        $('.autodropdown').hide();
        var chosenInput2 = document.querySelector('.autoarrival input');
        chosenInput2.focus();
    });

    $(document.body).on('click', function (event) {
        if (!$(event.target).closest('.chosen-drop').length && !$(event.target).is('#myInput1')) {
            $('.chosen-drop').hide();
        }
    });

});

document.addEventListener('DOMContentLoaded', function () {
    $('.chosen-drop').css('display', 'none');
    var CityName = "Mumbai";
    var CityCode = "BOM";
    var AirportName = "Chhatrapati Shivaji Maharaj International Airport";
    document.getElementById("myInput1").value = CityName + "-" + CityCode;
    document.getElementById("airportArval").innerHTML = AirportName;
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
    $('.chosen-drop').hide();
    dropdown.selectedIndex = -1;
}



//**********Chosen Departure Code Start ****************//
//*****************************************************//
$(document).ready(function () {
    $('#selectedItemId').chosen();
    $('.chosen-drop').hide();
    $('#selectedItemId').trigger('chosen:open');

    $('#myInput').on('click', function (event) {
        event.stopPropagation();
        $('.chosen-drop').show();
        $('.chosen-search input').attr('placeholder', ' From');
        $('.autoarrival').hide();
        $('.autodropdown').show();
        var chosenInput = document.querySelector('.chosen-search input');
        chosenInput.focus();
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
    $('.chosen-drop').hide();
    dropdown.selectedIndex = -1;
}
//***********From Cityname DropDown end********************//






////*****-----------RoundTrip end date disabled--********------//
$(document).ready(function () {

    //****  Replace Icon JS Start*/
    $('.rplc-btn').on('click', function () {
        var flyingFromValue = $('#myInput').val();
        var flyingToValue = $('#myInput1').val();
        $('#myInput').val(flyingToValue);
        $('#myInput1').val(flyingFromValue);
        if (flyingFromValue == flyingToValue) {
            alert("Departure and Arrival Airport are same please change it.")
            return false;
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
            $('#bgEnddate').css('background-color', '#e9ecef');
            $('#end-date').css('visibility', 'hidden');
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
        if (count > 1) {
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