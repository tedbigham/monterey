
window.onload = () => {
	let ws;

	$('#connect').click(connect);
	$('#createUser').click(createUser);
	$('#login').click(login);
	$('#listRooms').click(listRooms);
	$('#createRoom').click(createRoom);
	$('#deleteRoom').click(deleteRoom);
    $('#joinRoom').click(joinRoom);
    $('#listUsers').click(listUsers);
	$('#sendMessage').click(sendMessage);
	$('#clear').click(clear);

	function connect() {
		ws = new WebSocket('ws://localhost:5000');
		ws.onopen = () => {
			$('#connection').text('Open');
			log('websocket opened');
			$('.wsgroup').prop('disabled', false);
		};
		ws.onerror = event => {
			console.error('error: ' + event);
			$('#connection').text('Error');
			log('websocket error');
		};
		ws.onclose = event => {
			$('#connection').text('Closed');
			log('websocket closed');
			$('.wsgroup').prop('disabled', true);
		};
		ws.onmessage = event => {
			log(event.data);
		};
	}

	function send(message) {
		var json = JSON.stringify(message);
		log(json);
		return ws.send(json);
	}

	function log(message) {
		console.log(message);
		$('#messages').append(message + "\n");
		var scroll = $('#scroll');
		 scroll.scrollTop( scroll.prop("scrollHeight"));
	}

	function createUser() {
		var auth = {
			name: $("#newName").val(),
			password: $("#newPassword").val()
		};
		var json = JSON.stringify(auth);
		log(json);

		$.ajax({
			url: "api/users",
			data: json,
			method: "POST",
			contentType:"application/json; charset=utf-8"
		}).done(success => log(JSON.stringify(success))
		).fail(data => log(data.status + " " + data.statusText + " " + data.responseText + "\n"));
	}

	function login() {
		send({
			op: "auth",
			name: $('#user').val(),
			password: $('#password').val()
		});
	}
    function listRooms() {
		send({
			op: "list-rooms",
		});
    }
    function createRoom() {
		send({
			op: "create-room",
            room: $('#room').val()
		});
		setTimeout(joinRoom, 1000);
    }
	function deleteRoom() {
		send({
			op: "delete-room",
            room: $('#room').val()
		});
	}
    function joinRoom() {
        send({
            op: "join-room",
            room: $('#room').val()
        });
    }    
    function listUsers() {
        send({
            op: "list-users",
            room: $('#room').val()
        });
    }    
	function sendMessage() {
		send({
			op: "message",
			room: $('#chatRoom').val(),
			message: $('#message').val()
		});
	}
	function clear() {
		$('#messages').text('');
	}
}
