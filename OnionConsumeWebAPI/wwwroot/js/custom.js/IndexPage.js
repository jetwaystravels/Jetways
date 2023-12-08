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
                        // Enable the end date input field when Round Trip is selected
                        $('#end-date').prop('disabled', false);
                    } else {
                        // Disable the end date input field for other options
                        $('#end-date').prop('disabled', true);
                    }
                });

                ////*****-----------DatePicker Start*****-----------//
                $(function () {

                    $("#start-date").datepicker(
                        {
                            dateFormat: 'yy-mm-dd',
                            numberOfMonths: 2,
                            maxDate: '+2m',
                            minDate: '0'
                        });
                    $("#end-date").datepicker(
                        {
                            dateFormat: 'yy-mm-dd',
                            numberOfMonths: 2,
                            maxDate: '+3m',
                            minDate: '0'
                        });
                });

                //*****-----------Travell Detail , Adult Count, Child Count, Infant Count!!----*******//
                var maxField = 6;
                var maxField_adult = 9;
                var count = 0;
                $('.increment').on('click', function () {
                    if (count < maxField_adult) {
                        count++;
                        $('#count_adult').text(count);
                        $('#field_adult').val(count);
                    }
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
                        }
                    });




