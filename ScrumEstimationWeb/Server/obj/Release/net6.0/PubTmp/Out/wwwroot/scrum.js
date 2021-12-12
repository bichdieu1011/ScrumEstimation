function copyRoomIdClipboard() {
    /* Get the text field */
    var copyText = document.getElementById("room-id");
    /* Select the text field */
    copyText.select();

    var text = copyText.value;

    var el = document.createElement('textarea');    // Create a <textarea> element
    el.value = text;                                 // Set its value to the string that you want copied
    el.setAttribute('readonly', '');                // Make it readonly to be tamper-proof
    el.style.position = 'absolute';
    el.style.left = '-9999px';                      // Move outside the screen to make it invisible
    document.body.appendChild(el);                  // Append the <textarea> element to the HTML document
    var selected =
        document.getSelection().rangeCount > 0        // Check if there is any content selected previously
            ? document.getSelection().getRangeAt(0)     // Store selection if found
            : false;                                    // Mark as false to know no selection existed before
    el.select();                                    // Select the <textarea> content
    document.execCommand('copy');                   // Copy - only works as a result of a user action (e.g. click events)
    document.body.removeChild(el);                  // Remove the <textarea> element
    if (selected) {                                 // If a selection existed before copying
        document.getSelection().removeAllRanges();    // Unselect everything on the HTML document
        document.getSelection().addRange(selected);   // Restore the original selection
    }

    console.log("Copied the text: " + text);
}

function setRoomOwner() {
    var roomId = $("#room-id").val();
    localStorage.setItem("roomId-" + roomId, true);
}


function getALlLocalStorage() {
    var result = [];
    for (var item in localStorage) {
        if (item.includes("roomId-")) {
            var getValue = localStorage.getItem(item);
            if (getValue == "true") {
                result.push(item.replace("roomId-", ""));
                console.log(item.replace("roomId-", ""));
            }
        }
    }
    return result;
}

var chart;
function OnGenerateChart() {
    var color = ['rgba(204, 255, 245)',//0
        'rgba(204, 204, 255)',//0.5
        'rgba(128, 128, 255)',//1
        'rgba(163, 102, 255)',//2
        'rgba(224, 133, 194)',//3
        'rgba(203, 52, 153)',//5
        'rgba(204, 41, 0)',//8
        'rgba(128, 26, 0)',//13
        'rgba(102, 102, 153)',
        'rgba(0, 153, 204)'];

    var data = $("#chartData").val();
    data = JSON.parse(data);

    var ctx = document.getElementById('estimateReport').getContext('2d');
    var data = {
        labels: ['0', '0.5', '1', '2', '3', '5', '8', '13', '21', '99'],
        datasets: [{
            data: data,
            backgroundColor: color,
            borderColor: color,
            borderWidth: 1
        }]
    };
    if (chart != null)
        chart.destroy();
    chart = new Chart(ctx, {
        type: 'pie',
        data: data,
        options: {
            plugins: {
                title: {
                    display: true,
                    text: 'Custom Chart Title',
                    padding: {
                        top: 10,
                        bottom: 30
                    }
                }
            }
        }
    });


}


function receive(id, user, message) {
    console.log(id + user + message);
}