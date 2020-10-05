var express = require('express');
var router = express.Router();
var db = require('../db');

// Get client IP address from request object ----------------------
getClientAddress = function (req) {
  return (req.headers['x-forwarded-for'] || '').split(',')[0] 
  || req.connection.remoteAddress;
};

/* GET users listing. */
router.get('/', function(req, res, next) {
  res.json({
    ip: getClientAddress(req)
  });
});

router.get('/player/:mac', function(req, res, next) {
  var ip = getClientAddress(req);
  db.getPlayer(ip, req.params.mac).then((row) => {
    if (row == undefined)
    {
      res.json({
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

router.post('/playerFinished', function(req, res, next) {
  var ip = getClientAddress(req);
  db.insertPlayer(ip, req.body.macAddr, req.body.message, req.body.secondsPlayed, req.body.playerName, req.body.playerFavoriteColor, 1)
  .then((value) => {
    res.send("Success!");
  })
  .catch((err) => {
    res.send("Error: " + err.toString());
  });
});

module.exports = router;
