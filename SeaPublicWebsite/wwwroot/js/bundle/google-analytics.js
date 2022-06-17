function selectAndCopy() {
    document.addEventListener("copy", () => {
        let copiedText = document.getSelection().toString();
        gtag("event", "copy_text", {
            "copied_text": copiedText
        });
    });
}

function changeInputValue() {
    const inputs = document.getElementsByTagName("input");
    const inputAnswers = new Map();

    for (const input of inputs) {
        input.addEventListener("change", () => {
            let name = input.name;
            let value = input.value;
            let type = input.type;
            
            if (type === "checkbox") {
                // Send a hit when a checkbox is unchecked
                if (!input.checked) {
                    gtag("event", `change_input_${type}`, {
                        "name": name,
                        "value": value,
                    });
                }
            } else if (type === "radio" || type === "text") {
                // Send a hit if the inputs corresponding to that name already had a value
                if (inputAnswers.has(name)) {
                    gtag("event", `change_input_${type}`, {
                        "name": name,
                        "value": value,
                        "previous_value": inputAnswers.get(name)
                    });
                }
                inputAnswers.set(name, value);
            }
        });
    }
}

function backLink() {
    const backButton = document.getElementById("back-link");
    if (backButton !== null) {
        backButton.addEventListener("click", () => {
            gtag("event", "back_button_pressed")
        });
    }
}

function expandDropDown() {
    const details = document.getElementsByTagName("details");
    for (const detail of details) {
        detail.addEventListener("toggle", () => {
            if (detail.open) {
                // Send a hit when the user expands the drop-down
                const summaries = detail.getElementsByTagName("summary");
                if (summaries.length > 0) {
                    // We don't care about other summary elements other than the very first child of the details tag
                    const summary = summaries[0];
                    gtag("event", "expand_drop_down", {
                        "value": summary.innerText
                    });
                }
            }
        });
    }
}

function clickExternalLink() {
    const anchors = document.getElementsByTagName("a");
    for (const anchor of anchors) {
        if (!anchor.href.includes(document.location.hostname)) {
            // Send a hit if the link is clicked and redirects to an external site
            anchor.addEventListener("click", () => {
                gtag("event", "visit_external_site", {
                    "name": anchor.innerText,
                    "value": anchor.href
                });
            });
        }
    }
}

function setUpGoogleAnalytics() {
    selectAndCopy();
    changeInputValue();
    backLink();
    expandDropDown();
    clickExternalLink();
}

setUpGoogleAnalytics();