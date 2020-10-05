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
db.run("CREATE TABLE IF NOT EXISTS players (ip TEXT, mac TEXT, finished INTEGER, PRIMARY KEY(ip))").then((result) => {
    console.log("Tables set up");
    console.log(result);
});

var getPlayer = (ip, mac) => {
    return db.get("SELECT ip, mac, finished FROM players WHERE ip=?", [ip]).then((row) => {
        if (row == undefined) {
            return db.run("INSERT INTO players (ip, mac, finished) VALUES (?, ?, ?)", [ip, mac, 0]).then((result) => {
                return db.get("SELECT ip, mac, finished FROM players WHERE ip=?", [ip]);
            });
        }
        return row;
    });
};

module.exports = {getPlayer};
 

