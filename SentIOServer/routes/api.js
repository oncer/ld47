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

router.get('/player', function(req, res, next) {
  var ip = getClientAddress(req);
  db.getPlayer(ip).then((row) => {
    res.json(row);
  }).catch((err) =>{
    console.log(err);
    res.json({error: err.toString()});
  });
});

module.exports = router;
