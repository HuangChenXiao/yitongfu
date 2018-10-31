var merge = require('webpack-merge')
var prodEnv = require('./prod.env')

module.exports = merge(prodEnv, {
    NODE_ENV: '"development"',
    // BASE_API: '"http://localhost:59358/api/"',
    BASE_API: '"http://admin.az818.com/Api/api/"',
    // JAVA_API: '"http://192.168.0.9:8080/"',
    JAVA_API: '"http://payapi.az818.com/"',
})