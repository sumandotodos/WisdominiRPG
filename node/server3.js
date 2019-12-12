var express    = require("express");
var mysql      = require('mysql');
var nodemailer = require('nodemailer');
var connection = mysql.createConnection({
  host     : 'wisdomini.flygames.org',
  user     : 'root',
  password : 'ninguna2',
  database : 'Wisdomini'
});
var app = express();
var bodyParser = require('body-parser');
app.use(bodyParser.urlencoded({ extended: true }));


connection.connect(function(err){
if(!err) {
    console.log("Database is connected ... nn");    
} else {
    console.log("Error connecting database ... nn");    
}
});

app.use(require('body-parser').urlencoded());
//app.use(express.urlencoded());

connection.query('use Wisdomini');

// create reusable transporter object using the default SMTP transport
var transporter = nodemailer.createTransport('smtps://emilio.pomares.porras%40gmail.com:Sacramento31416@smtp.gmail.com');

// setup e-mail data with unicode symbols
var mailOptions = {
    from: '"Fred Foo" <foo@blurdybloop.com>', // sender address
    to: 'kazlivsjy@yahoo.com', // list of receivers
    subject: 'Hello', // Subject line
    text: 'Hello world', // plaintext body
    html: '<b>Hello world</b>' // html body
};


app.post("/newUser",function(req,res){

	connection.query('insert into Users (uuid, email) values ("' + req.body.uuid + '", "' + req.body.email + '")',
	function selectCb(err, results, fields) {
    	if (err) throw err;
    	else res.sendStatus(200);
	});



});


app.get("/newUser",function(req,res){

        connection.query('insert into Users (uuid, email) values ("' + req.body.uuid + '", "' + req.body.email + '")',
        function selectCb(err, results, fields) {
        if (err) throw err;
        else res.sendStatus(200);
        });



});

app.post('/requestMail',function(req,res){

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


app.listen(8080);

