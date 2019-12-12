var express    = require("express");
var mysql      = require('mysql');
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

app.post("/",function(req,res){

connection.query('insert into Users (uuid, email) values ("' + req.body.uuid + '", "' + req.body.email + '")',
function selectCb(err, results, fields) {
    if (err) throw err;
    else res.sendStatus(200);
});

//console.log(req.body.uuid);

//res.sendStatus(200);

});


app.listen(8080);

