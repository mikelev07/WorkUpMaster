$(function () {
    $('#chatroom').scrollTop($('#chatroom').prop('scrollHeight'));
        // Ссылка на автоматически-сгенерированный прокси хаба
        var chat = $.connection.chatHub;
    
        chat.client.displayMessage = function (message) {
            $('#notification').html(message);
        };

        // Объявление функции, которая хаб вызывает при получении сообщений
        chat.client.addMessage = function (name, message, partnerId) {
        if (htmlEncode(name) == $('#username').val()) {
            var messageHtml = '<div class="message-bubble me"><div class="message-bubble-inner"><div class="message-avatar"><img src="/Content/Custom/images/user-avatar-small-01.jpg" alt="" /></div><div class="message-text"><p>' + htmlEncode(message) + '</p></div></div><div class="clearfix"></div></div>';
        } else {
            var messageHtml = '<div class="message-bubble"><div class="message-bubble-inner"><div class="message-avatar"><img src="/Content/Custom/images/user-avatar-small-01.jpg" alt="" /></div><div class="message-text"><p>' + htmlEncode(message) + '</p></div></div><div class="clearfix"></div></div>';
        }
        
        if ($('#toUserName').val() != '') {
            $('#chatroom').append(messageHtml);
        }
        else
        {
            alert("new message");
        }
        
        // Добавление сообщений на веб-страницу 
       // $('#chatroom').append(messageHtml);
      
        $('#chatroom').scrollTop($('#chatroom').prop('scrollHeight'));
    };

        // Функция, вызываемая при подключении нового пользователя
        chat.client.onConnected = function (id, userName, allUsers) {

                $('#chatBody').show();
                // установка в скрытых полях имени и id текущего пользователя
                $('#hdId').val(id);
                $('#username').val(userName);
           

                // Добавление всех пользователей
                for (i = 0; i < allUsers.length; i++) {

                    AddUser(allUsers[i].ConnectionId, allUsers[i].UserName);

                }
        }

        // Добавляем нового пользователя
        chat.client.onNewUserConnected = function (id, name) {

            AddUser(id, name);
            if ($('#partnerId').val() != '') {

            }
        }

        // Удаляем пользователя
        chat.client.onUserDisconnected = function (id, userName) {

            //   $('#' + id).remove();
            var htmlName = '#' + userName + '-status';
            var conidName = userName + '-conid';
            $(htmlName).attr('class', 'status-icon status-offline');
           // $(conidName).attr('class', 'status-icon status-offline');
            $('.' + id).remove();
            $('#' + id).attr('id', conidName);
            $('#partnerId').val() = id;
    }

  
        // Открываем соединение
        $.connection.hub.start().done(function () {
            chat.server.connect();
                $('#sendmessage').click(function () {
            // Вызываем у хаба метод Send
            if ($('#toUserName').val() != '') {
                chat.server.send($('#username').val(), $('textarea#message').val(), $('#partnerId').val(), $('#toUserName').val());
                $('textarea#message').val('');
            }
            else {
                Snackbar.show({
                    text: 'Необходимо выбрать диалог',
                });
            }
            });  

            $('#message').keypress(function (e) {
                var key = e.which;
                if (key == 13)  // the enter key code
                {
                    if ($('#toUserName').val() != '') {
                        chat.server.send($('#username').val(), $('textarea#message').val(), $('#partnerId').val(), $('#toUserName').val());
                        $('textarea#message').val('');
                        return false;
                    }
                }
            });
        });
});

function setValue(id) {
    document.getElementById('partnerId').value = id;
}

function loadHistory(name) {
    document.getElementById('toUserName').value = name;
    $.ajax({
        url: '/Chat/LoadHistory',
        type: 'GET',
        cache: false,
        data: { userName: name }
    }).done(function (result) {
        $('#chatroom').html(result);
        $('#chatroom').scrollTop($('#chatroom').prop('scrollHeight'));
    });

}

// Кодирование тегов
function htmlEncode(value) {
        var encodedValue = $('<div />').text(value).html();
        return encodedValue;
}
//Добавление нового пользователя
function AddUser(id, name) {
    var userIdStr = $('#hdId').val();
    if (userIdStr != id) {
       //<input type="button" onclick="javascript:setValue(' + sId + ');" value="Начать чат" class="' + id + '">')
       //var userHtml = '<li id="' + id + '"onclick="javascript:setValue(' + sId + ',' + stringUserId + ');"><a href="#" ><div class="message-avatar"><i class="status-icon status-online"></i><img src="images/user-avatar-small-03.jpg" alt="" /></div><div class="message-by"><div class="message-by-headline"><h5>' + name + '</h5><span>4 hours ago</span></div><p>Thanks for reaching out. Im quite busy right now on many</p></div></a></li>';
        //$("#chatusers").append(userHtml);
        var htmlName = '#' + name + '-status';
        var conidName = '#' + name + '-conid';
        $(htmlName).attr('class', 'status-icon status-online');
        $(conidName).attr('id', id);
       // $(conidName).on('click',setValue(name,id));
       // "javascript:@(Html.Raw(String.Format("setValue({0},{1})","'" + d.UserTo.UserName + "'","'" + d.UserTo.ConnectionId + "'")))"
    }

        // $('#chatroom').load('@Url.Action("LoadHistory", "Chat")?id=' + id + '?myId='      
}

