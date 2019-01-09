var merge = require('webpack-merge')
var prodEnv = require('./prod.env')

module.exports = merge(prodEnv, {
    NODE_ENV: '"development"',
    BASE_API: '"http://localhost:59358/api/"',
    // BASE_API: '"http://admin.az818.com/Api/api/"',
    JAVA_API: '""',
    EXCEL_API:'"http://localhost:59358/Export/"'
})