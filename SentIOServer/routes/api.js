var express = require('express');
var router = express.Router();
var db = require('../db');

/* GET users listing. */
router.get('/', function(req, res, next) {
  res.json({
    ip: db.getClientAddress(req)
  });
});

router.get('/player/:mac', function(req, res, next) {
  var ip = db.getClientAddress(req);
  db.getPlayer(ip, req.params.mac).then((row) => {
    if (row == undefined)
    {
      res.json({
        ip: ip,
        finished: false
      })
    }
    else
    {
      res.json(row);
    }
  }).catch((err) =>{
    console.log(err);
    res.json({error: err.toString()});
  });
});

router.get('/finishedPlayers', function(req, res, next) {
  db.getFinishedPlayers().then((rows) => {
    res.json(rows);
  })
  .catch((err) => {
    res.json({error: err.toString()});
  });
});

router.post('/playerFinished', function(req, res, next) {
  var ip = db.getClientAddress(req);
  db.insertPlayer(ip, req.body.macAddr, req.body.message, req.body.secondsPlayed, req.body.playerName, req.body.playerFavoriteColor, 1)
  .then((value) => {
    res.send("Success!");
  })
  .catch((err) => {
    res.send("Error: " + err.toString());
  });
});

module.exports = router;
