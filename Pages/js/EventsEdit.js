//
//
//



// ======================================================
//  Create Tiebreaker Objects
// ======================================================
document.addEventListener("DOMContentLoaded", function () {
    const tiebreakerLabels = [
        "Primary",
        "Secondary",
        "Tertiary",
        "Quaternary",
        "Quinary",
    ];
    const tiebreakerContainer = document.getElementById("tiebreaker-options");

    // Dropdown options
    const options = [
        "Points Head to Head",
        "W/T/L Head to Head",
        "Games Won Overall",
        "Matches Forfeited",
        "None",
    ];

    tiebreakerLabels.forEach((label, index) => {
        // Create a container for each dropdown
        const dropdownContainer = document.createElement("div");
        dropdownContainer.classList.add("dropdown-container");
        dropdownContainer.id = `${label.toLowerCase()}-tiebreaker`;

        // Create a label for the dropdown
        const labelElement = document.createElement("p");
        labelElement.classList.add("option-label");
        labelElement.innerText = label;
        dropdownContainer.appendChild(labelElement);

        // Create the select element (dropdown)
        const selectElement = document.createElement("select");
        selectElement.classList.add("Dropdown");
        selectElement.name = `${label.toLowerCase()}-tiebreaker`;
        selectElement.dataset.order = index; // Set the correct order on the select element

        options.forEach((optionText, optionIndex) => {
            const optionElement = document.createElement("option");
            optionElement.value = optionText;
            optionElement.innerText = optionText;

            // Set the default selected option based on the index of the label
            if (optionIndex === index) {
                optionElement.selected = true; // Set as selected
            }

            selectElement.appendChild(optionElement);
        });

        // Add the change event listener to the select element
        selectElement.addEventListener("change", handleTiebreakerChange);

        // Append the select element to the dropdown container
        dropdownContainer.appendChild(selectElement);

        // Append the dropdown container to the main container
        tiebreakerContainer.appendChild(dropdownContainer);
    });

    // Event handler for when a tiebreaker option is changed
    function handleTiebreakerChange(event) {
        const tiebreakerDiv = document.getElementById("tiebreaker-options");
        const dropdowns = tiebreakerDiv.querySelectorAll("select");
        var option = event.target;

        // If "None" is selected, move all selections below it up
        if (option.value === "None") {
            handleNone(option, dropdowns);
        }
        // Otherwise, remove any same selection in previous dropdowns
        else {
            dropdowns.forEach((dropdown) => {
                const optionOrder = parseInt(option.dataset.order); // Correctly reference the option's order
                const dropdownOrder = parseInt(dropdown.dataset.order); // Access dropdown's data-order

                // If the dropdown is below the current option, check to see if it is the same value
                if (dropdownOrder > optionOrder) {
                    // If the dropdown value is the same as the current option, reset it to "None"
                    if (dropdown.value === option.value) {
                        dropdown.value = "None"; // Reset to "None"
                        handleNone(dropdown, dropdowns); // Call handleNone to adjust any further dropdowns below it
                    }
                }
                // If the dropdown is above the current option, check to see if it is the same value
                else if (dropdownOrder < optionOrder) {
                    if (dropdown.value === option.value) {
                        option.value = "None"; // Reset to "None"
                        handleNone(option, dropdowns); // Call handleNone to adjust any further dropdowns below it
                    }
                    // If the dropdown above the selection is None then swap this with that one
                    else if (dropdown.value === "None") {
                        dropdown.value = option.value;
                        option.value = "None";
                        handleNone(option, dropdowns);
                    }
                }
            });
        }
    }

    function handleNone(option, dropdowns) {
        dropdowns.forEach((dropdown) => {
            const optionOrder = parseInt(option.dataset.order); // Correctly reference the option's order
            const dropdownOrder = parseInt(dropdown.dataset.order); // Access dropdown's data-order

            //If the option is last then we keep it None and don't swap
            if (optionOrder === dropdowns.length - 1) return;
            // If the dropdown is above the current option, skip it
            else if (dropdownOrder <= optionOrder) return;
            // If the dropdown is already "None", skip it
            else if (dropdown.value === "None") return;

            // Swap the values of the index and dropdown
            const tempValue = option.value; // Store the value of the current `option`
            option.value = dropdown.value;
            dropdown.value = tempValue;

            // Update the option reference if swapped
            option = dropdown; // This keeps `option` pointing to the current dropdown
        });
    }
});

// ======================================================
//  Adding W/T/L Points Values Buttons
// ======================================================
const pointsHeading = document.getElementById("wtl-values");

const divNames = ["win", "tie", "loss", "forfeit"];
const divs = Object.fromEntries(
    divNames.map((name) => [name, document.createElement("div")])
);

// Create divs with labels, buttons, and a counter
divNames.forEach((name) => {
    const div = divs[name];
    div.id = name + "div"; // Assign the div ID dynamically
    div.style.margin = "10px"; // Add spacing for visibility

    // Create label for the category
    const labelText = document.createElement("label");
    labelText.style = "width:5rem; text-align: right;";
    labelText.textContent = name.charAt(0).toUpperCase() + name.slice(1) + ": "; // Capitalize first letter
    labelText.setAttribute("for", div.id); // Associate the label with the div

    // Create counter span
    const counter = document.createElement("span");
    counter.id = name + "counter"; // Assign the counter ID dynamically
    counter.textContent = "0"; // Initial value
    if (name === "win") counter.textContent = "2";
    if (name === "tie") counter.textContent = "1";

    counter.style.margin = "0 10px"; // Add spacing

    // Create increment button
    const incButton = document.createElement("button");
    incButton.classList.add("inc-dec-button");
    incButton.textContent = "+";
    incButton.addEventListener("click", (event) => {
        event.preventDefault(); // Prevent default behavior
        counter.textContent = parseInt(counter.textContent) + 1;
    });

    // Create decrement button
    const decButton = document.createElement("button");
    decButton.classList.add("inc-dec-button");
    decButton.textContent = "-";
    decButton.addEventListener("click", (event) => {
        event.preventDefault(); // Prevent default behavior
        counter.textContent = Math.max(0, parseInt(counter.textContent) - 1); // Prevent negative values
    });

    // Append elements to div
    div.appendChild(labelText);
    div.appendChild(decButton);
    div.appendChild(counter);
    div.appendChild(incButton);

    // Append div to pointsHeading
    pointsHeading.appendChild(div);
});

// ======================================================
//  Setting the hover text on heading hover
// ======================================================
// Function to handle the hover event and update hover-text
function handleHoverEvent(event) {
    const textdiv = document.getElementById("hover-text");
    const hoveredElement = event.target; // Get the element that triggered the event

    // Check if the hovered element has a 'data-tip' attribute
    if (hoveredElement.hasAttribute("data-tip")) {
        // Get the tip from the data-tip attribute of the heading
        const tipText = hoveredElement.getAttribute("data-tip");

        // Update the hover-text with the appropriate tip
        textdiv.innerText = tipText;
    }
}

// Get all left-side headings that have data-tip and data-target
const leftHeadings = document.querySelectorAll("#settings-list h3[data-tip]");

// Get all right-side headings
const rightHeadings = document.querySelectorAll("#settings-options h3");

// Add the hover event listener to both left and right headings
[...leftHeadings, ...rightHeadings].forEach((heading) => {
    heading.addEventListener("mouseover", handleHoverEvent);
});
