var formatDate = function (d = new Date()) {//返回年月日
  var now = d;
  var year = now.getFullYear();
  var month = now.getMonth() + 1;
  var date = now.getDate();
  month=parseInt(month) < 10?"0"+month:month;
  date=parseInt(date) < 10?"0"+date:date;
  return year + "-" + month + "-" + date;
}
var formatTime = function (d = new Date()) {//返回年月日时分秒
  var now = d;
  var year = now.getFullYear();
  var month = now.getMonth() + 1;
  var date = now.getDate();
  var hour = now.getHours() < 10 ? "0" + now.getHours() : now.getHours();
  var minute = now.getMinutes() < 10 ? "0" + now.getMinutes() : now.getMinutes();
  var second = now.getSeconds() < 10 ? "0" + now.getSeconds() : now.getSeconds();
  return year + "-" + month + "-" + date + " " + hour + ":" + minute + ":" + second;
}
//获取所选月份的第一天
var getCurrentMonthFirst = function (d = new Date()) {
  var now = new Date(d);
  now.setDate(1);
  var year = now.getFullYear();
  var month = (now.getMonth() + 1) < 10 ? "0" + (now.getMonth() + 1) : (now.getMonth() + 1);
  var date = now.getDate() < 10 ? "0" + now.getDate() : now.getDate();
  var hour = now.getHours() < 10 ? "0" + now.getHours() : now.getHours();
  var minute = now.getMinutes() < 10 ? "0" + now.getMinutes() : now.getMinutes();
  var second = now.getSeconds() < 10 ? "0" + now.getSeconds() : now.getSeconds();
  return year + "-" + month + "-" + date + " " + hour + ":" + minute + ":" + second;
};
//获取所选月份的最后一天
var getCurrentMonthLast = function (d = new Date()) {
  var endDate = new Date(d);
  var month = endDate.getMonth();
  var nextMonth = ++month;
  var nextMonthFirstDay = new Date(endDate.getFullYear(), nextMonth, 1);
  var oneDay = 1000 * 60 * 60 * 24;
  var now = new Date(nextMonthFirstDay - oneDay);
  var year = now.getFullYear();
  var month = (now.getMonth() + 1) < 10 ? "0" + (now.getMonth() + 1) : (now.getMonth() + 1);
  var date = now.getDate() < 10 ? "0" + now.getDate() : now.getDate();
  var hour = now.getHours() < 10 ? "0" + now.getHours() : now.getHours();
  var minute = now.getMinutes() < 10 ? "0" + now.getMinutes() : now.getMinutes();
  var second = now.getSeconds() < 10 ? "0" + now.getSeconds() : now.getSeconds();
  return year + "-" + month + "-" + date + " " + hour + ":" + minute + ":" + second;
};
//json转url参数
var  parseParams=function(data) {
  try {
      var tempArr = [];
      for (var i in data) {
          var key = encodeURIComponent(i);
          var value = encodeURIComponent(data[i]);
          tempArr.push(key + '=' + value);
      }
      var urlParamsStr = tempArr.join('&');
      return urlParamsStr;
  } catch (err) {
      return '';
  }
}  
export default {
  formatDate,
  formatTime,
  getCurrentMonthFirst,
  getCurrentMonthLast,
  parseParams
}
