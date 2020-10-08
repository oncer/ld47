var express = require('express');
var router = express.Router();
var db = require('../db');

/* GET home page. */
router.get('/', function(req, res, next) {
  var ip = db.getClientAddress(req);
  db.getFinishedPlayerByIp(ip).then((player => {
    //console.log(player);
    params = { title: "Jack's mementos" };
    var mac = "NULL";
    if (player == undefined)
    {
      params["finished"] = 0;
    }
    else
    {
      params["finished"] = 1;
      params["name"] = player["name"];
      var seconds = player["secondsPlayed"];
      var minutes = Math.floor(seconds / 60);
      seconds -= minutes * 60;
      params["minutes"] = minutes;
      params["seconds"] = seconds;
      params["message"] = player["message"];
      mac = player["mac"];
    }
    db.getOtherFinishedPlayers(ip, mac).then((rows => {
      params["players"] = rows;
      res.render('index', params);
    }));
  }));
});

module.exports = router;
