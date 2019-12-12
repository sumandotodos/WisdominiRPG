var express    = require("express");
var nodemailer = require('nodemailer');
var app = express();
var bodyParser = require('body-parser');
app.use(bodyParser.urlencoded({ extended: true }));

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

app.use(require('body-parser').urlencoded());
//app.use(express.urlencoded());


app.post('/q',function(req,res){

// send mail with defined transport object
transporter.sendMail(mailOptions, function(error, info){
    if(error){
        return console.log(error);
    }
    console.log('Message sent: ' + info.response);
});

res.sendStatus(200);
	

});




app.listen(8080);

