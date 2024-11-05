
    $(document).ready(function () {
      
        const connection = new signalR.HubConnectionBuilder()
    .withUrl("/alertHub")
    .build();

       
        connection.start().then(() => {
        console.log("SignalR Connected");
        }).catch(err => {
        console.error("SignalR Connection Error: ", err);
        });

       
        connection.on("ReceiveMessage", (user, message) => {
          

                $('#alertmessage').append(` <div class="alert alert-success zindex-1" role="alert">
                <h4 class="alert-heading">FROM ADMIN </h4>
                <p>${message}</p >
            </div>`);

            $('#notifymessageprofile').append(`
                <h2 class="bigtextadmin">From Admin</h2>
                <p class="bigtext">${message}</p>`);
            var alertElement = $('.alert-success');
            $('.myspan').removeClass('hidenotify');

            function removeAlert() {
                if (alertElement.length) {
                    alertElement.remove();
                }
            }
            setTimeout(removeAlert, 10000);
        });


    function sendAlert(message) {

        connection.invoke("SendAlert", message).catch(err => {
            console.error("SignalR  Error: ", err);
        });
        }


        $('#click').click(function () {
            const message = $('#getmessage').val();
            sendAlert(message);
            $('#getmessage').val('');
        });
    });

