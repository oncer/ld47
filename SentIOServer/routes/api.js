var express = require('express');
var router = express.Router();
var db = require('../db');

/* GET users listing. */
router.get('/', function(req, res, next) {
  res.json({
    ip: req.ip
  });
});

router.get('/player', function(req, res, next) {
  db.getPlayer(req.ip).then((row) => {
    console.log("getPlayer...");
    console.log(row);
    res.json(row);
  }).catch((err) =>{
    console.log(err);
    res.json({error: err});
  });
});

module.exports = router;
