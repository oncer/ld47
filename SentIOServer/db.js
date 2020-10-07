var sqlite3 = require('sqlite3').verbose();

class AppDAO
{
    constructor(dbFilePath) {
        this.db = new sqlite3.Database(dbFilePath, (err) => {
            if (err) {
            console.log('Could not connect to database', err)
            } else {
            console.log('Connected to database')
            }
        })
        }

    run(sql, params = []) {
        return new Promise((resolve, reject) => {
            this.db.run(sql, params, function (err) {
            if (err) {
                console.log('Error running sql ' + sql)
                console.log(err)
                reject(err)
            } else {
                resolve({ id: this.lastID })
            }
            })
        })
        }

    get(sql, params = []) {
        return new Promise((resolve, reject) => {
            this.db.get(sql, params, (err, result) => {
            if (err) {
                console.log('Error running sql: ' + sql)
                console.log(err)
                reject(err)
            } else {
                resolve(result)
            }
            })
        })
        }
    
    all(sql, params = []) {
        return new Promise((resolve, reject) => {
            this.db.all(sql, params, (err, rows) => {
            if (err) {
                console.log('Error running sql: ' + sql)
                console.log(err)
                reject(err)
            } else {
                resolve(rows)
            }
            })
        })
        }
}

var db = new AppDAO("db.sqlite3");
db.run(`CREATE TABLE IF NOT EXISTS players (
    ip TEXT, 
    mac TEXT, 
    message TEXT, 
    secondsPlayed INTEGER, 
    name TEXT, 
    color TEXT, 
    finished INTEGER,
    timestamp TEXT
    )`).then((result) => {
    console.log("Tables set up");
    console.log(result);
});

// Get client IP address from request object ----------------------
var getClientAddress = function (req) {
    return (req.headers['x-forwarded-for'] || '').split(',')[0] 
    || req.connection.remoteAddress;
  };

var getPlayer = (ip, mac) => {
    return db.get("SELECT name, finished, ip, timestamp FROM players WHERE ip=? AND mac=? ORDER BY timestamp DESC", [ip, mac]);
};

var getFinishedPlayerByIp = (ip) => {
    return db.get("SELECT name, message, color, secondsPlayed, timestamp, finished FROM players WHERE ip=? AND finished=1 ORDER BY timestamp DESC", [ip]);
}

var insertPlayer = (ip, mac, message, secondsPlayed, name, color, finished) => {
    return db.run("INSERT INTO players (ip, mac, message, secondsPlayed, name, color, finished, timestamp) VALUES (?, ?, ?, ?, ?, ?, ?, datetime('now'))",
        [ip, mac, message, secondsPlayed, name, color, finished]);
};

var getFinishedPlayers = () => {
    return db.all(`SELECT name, message, color, secondsPlayed, timestamp FROM players l
    INNER JOIN (SELECT MAX(timestamp) as latest, ip, mac FROM players GROUP BY ip, mac) r
    ON l.timestamp = r.latest AND l.ip = r.ip AND l.mac = r.mac
    ORDER BY timestamp DESC`);
}

var getOtherFinishedPlayers = (ip) => {
    return db.all(`SELECT name, message, color, secondsPlayed, timestamp FROM players l
    INNER JOIN (SELECT MAX(timestamp) as latest, ip, mac FROM players GROUP BY ip, mac) r
    ON l.timestamp = r.latest AND l.ip = r.ip AND l.mac = r.mac
    WHERE l.ip <> ?
    ORDER BY timestamp DESC
    LIMIT 40`, [ip]);
}

module.exports = {getPlayer, insertPlayer, getFinishedPlayerByIp, getFinishedPlayers, getOtherFinishedPlayers, getClientAddress};
