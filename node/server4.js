var express    = require("express");
var mysql      = require('mysql');
var nodemailer = require('nodemailer');

var pool = mysql.createPool({
	connectionLimit : 255,
	host		: 'wisdomini.flygames.org',
	user		: 'root',
	password	: 'ninguna2',
	database	: 'Wisdomini'

});

var Ddos = require('ddos');
var ddos = new Ddos;

var app = express();
var bodyParser = require('body-parser');


app.use(require('express-toobusy')());

app.use(bodyParser.urlencoded({ extended: true }));

app.use(require('body-parser').urlencoded());

app.use(ddos.express);

// create reusable transporter object using the default SMTP transport
var transporter = nodemailer.createTransport('smtps://emilio.pomares.porras%40gmail.com:Sacramento31416@smtp.gmail.com');

app.get("/confirm", function(req, res) {

	pool.getConnection(function(err, connection) {

		if(err) {
			connection.release();
			return;
		}
	 	connection.query('select random, email from Requests where uuid = "' + req.query.uuid + '"' , function(err, rows) {
			if(err) {
				throw err;
			}
			if(rows.length > 0) {
				console.log('primer query');
				if(rows[0].random == req.query.random) {
						console.log('hemos llegado hasta aquí');

						connection.query('insert into Users (uuid, email) values ("' + req.query.uuid + '", "' + rows[0].email + 
							'")',
       						function selectCb(err, results, fields) {
       								if(err) throw err;
       								else {

       									res.sendStatus(200);

       								}
       					});

				}
			}
		});	

	});

});

app.post("/newUser",function(req,res){
	console.log('Attempting new user registration...');
	pool.getConnection(function(err, connection) {

		if(err) {
			connection.release();
			return;
		}
		require('crypto').randomBytes(20, function(ex, buf) { 
       			connection.query('replace into Requests (uuid, email, random) values ("' + req.body.uuid + '", "' + req.body.email + 
					'", "' + buf.toString('hex') + '")',
       				function selectCb(err, results, fields) {
       					if (err) throw err;
       					else { 
						var subject = 'Confirmación registro Wisdomini';
						var message = 'Si no has iniciado el proceso de registro del juego Wisdomini, puedes ignorar este mensaje' +
						'.<br><br>Para completar el registro, haz click en el siguiente enlace:<br>'+
						'<a href="' + 'http://wisdomini.flygames.org/confirm?uuid=' +
						req.body.uuid + '&r=' + buf.toString('hex')  + '">' + 
						'http://wisdomini.flygames.org/confirm?uuid=' +
                                                req.body.uuid + '&r=' + buf.toString('hex')
						+ '</a>';
						var newMailOptions = {
							from: 'Wisdomini <wisdomini@flygames.org>',
                                                                                to: req.body.email,
                                                                                subject: subject,
                                                                                text: message,
                                                                                html: message
						};	
						
						transporter.sendMail(newMailOptions, function(error, info){
                                                                                if(error){
                                                                                        return console.log(error);
                                                                                }
                                                                                res.sendStatus(200);
                                                });
					}
       			});
		});

	});

});

app.post('/requestMail',function(req,res){

	pool.getConnection(function(err, connection) {

		var user = req.body.uuid;
		var contid = req.body.contid;
	
		var recipient = "";
		var subject = "";
		var message = "";

		connection.query('select email from Users where uuid = "' + user +'"', 
			function(err, rows) {
				if(err) throw err;
				console.log(rows);
				console.log('rows[0].email = ' + rows[0].email);
				console.log('rows.length = ' + rows.length);
				if(rows.length > 0) {
					recipient = rows[0].email;	
					console.log('debug contents of recipient: ' + recipient);
					connection.query('select header, precontents, contents, postcontents from Contents where id = "' + contid + '"',
                				function(err, rows) {
                        				if(err) throw err;
                        				if(rows.length > 0) {
                                				subject = rows[0].header;
                                				message = rows[0].precontents + '\n\n' + 
									rows[0].contents + '\n\n' + 
									rows[0].postcontents;
 									var newMailOptions = {
							                	from: 'Wisdomini <wisdomini@flygames.org>',
                								to: recipient,
                								subject: subject,
                								text: message,
                								html: message
        								};

        								transporter.sendMail(newMailOptions, function(error, info){
         									if(error){
         										return console.log(error);
        									}
        									console.log('Message sent: ' + info.response);
        								});
                       				

								}
                					}
        					);

				}
			}
		);


		res.sendStatus(200);
	
	});

});


app.listen(80);

