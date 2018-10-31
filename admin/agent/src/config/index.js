var formatDate = function (d = new Date()) {//返回年月日
  var now = d;
  var year = now.getFullYear();
  var month = now.getMonth() + 1;
  var date = now.getDate();
  month=month<10?"0"+month:month;
  date= date<10?"0"+date:date;
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
export default {
  formatDate,
  formatTime,
  getCurrentMonthFirst,
  getCurrentMonthLast
}
export function formatTime(d = new Date()) {//返回年月日时分秒
  var now = d;
  var year = now.getFullYear();
  var month = now.getMonth() + 1;
  var date = now.getDate();
  var hour = now.getHours() < 10 ? "0" + now.getHours() : now.getHours();
  var minute = now.getMinutes() < 10 ? "0" + now.getMinutes() : now.getMinutes();
  var second = now.getSeconds() < 10 ? "0" + now.getSeconds() : now.getSeconds();
  return year + "-" + month + "-" + date + " " + hour + ":" + minute + ":" + second;
}
export function formatDate(d = new Date()) {//返回年月日
  var now = d;
  var year = now.getFullYear();
  var month = now.getMonth() + 1;
  var date = now.getDate();
  return year + "-" + month + "-" + date;
}
export function Today(d = new Date()) { //获取今天日期
  var now = d;
  var year = now.getFullYear();
  var month = now.getMonth() + 1;
  var date = now.getDate();
  var hour = now.getHours();
  var minute = now.getMinutes();
  var second = now.getSeconds();
  var r_date = {};
  r_date.d1 = year + "/" + month + "/" + date;
  r_date.d2 = year + "-" + month + "-" + date;
  return r_date;
}

export function getDateTime(d = new Date()) { //返回年月日
  var now = d;
  var year = now.getFullYear();
  var month = now.getMonth() + 1;
  var day = now.getDate();
  var hour = now.getHours();
  var minute = now.getMinutes();
  var second = now.getSeconds();
  var r_date = {};
  r_date.year = year
  r_date.month = month
  r_date.day = day
  return r_date;
}
export function Togettime(d = new Date()) { //获取当前时间
  var now = d;
  var year = now.getFullYear();
  var month = now.getMonth() + 1;
  month = month < 10 ? "0" + month : month;
  var date = now.getDate() < 10 ? "0" + now.getDate() : now.getDate();
  var hour = now.getHours() < 10 ? "0" + now.getHours() : now.getHours();
  var minute = now.getMinutes() < 10 ? "0" + now.getMinutes() : now.getMinutes();
  var second = now.getSeconds() < 10 ? "0" + now.getSeconds() : now.getSeconds();
  var r_date = {};
  r_date.d1 = year + "/" + month + "/" + date + ' ' + hour + ":" + minute + ":" + second;
  r_date.d2 = year + "-" + month + "-" + date + ' ' + hour + ":" + minute + ":" + second;
  return r_date;
}