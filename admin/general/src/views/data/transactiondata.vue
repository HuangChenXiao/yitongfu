<template>
  <div class="data-container">
    <el-row>
      <el-col :span="24">
        <!-- <el-button>昨天</el-button>
        <el-button type="primary">最近7天</el-button> -->
        <el-date-picker
          v-model="listQuery.begindate"
          type="date"
          placeholder="开始日期">
        </el-date-picker>
        <el-date-picker
          v-model="listQuery.enddate"
          type="date"
          placeholder="结束日期">
        </el-date-picker>
        <!-- <el-select v-model="ruleForm.type" placeholder="代理商商户">
          <el-option label="代理商" value="1"></el-option>
          <el-option label="商户" value="2"></el-option>
        </el-select> -->
        <el-input v-model="listQuery.keyword" placeholder="代理商名称/商户名称" style="width:200px"></el-input>
        <el-button type="primary" @click="getList()">搜索</el-button>
        <!-- <el-button type="primary">查看今日收益</el-button>
        <el-button type="primary">查看成本收益</el-button> -->
      </el-col>
      <el-col :span="24" class="grid">
          <el-col :span="8"><div class="grid-title">成交总笔数</div></el-col>
          <el-col :span="8"><div class="grid-title">实收总金额</div></el-col>
          <el-col :span="8"><div class="grid-title">代理商分佣</div></el-col>

          <el-col :span="8"><div class="grid-content">{{list.totaltradeqty}}</div></el-col>
          <el-col :span="8"><div class="grid-content">￥{{list.totalamount}}</div></el-col>
          <el-col :span="8"><div class="grid-content">￥{{list.totalcommission}}</div></el-col>

          <el-col :span="8"><div class="grid-echart" id="sumcount"></div></el-col>
          <el-col :span="8"><div class="grid-echart" id="summoney"></div></el-col>
          <el-col :span="8"><div class="grid-echart" id="sumcommission"></div></el-col>
      </el-col>
      <el-col :span="24" class="grid">
	  	  <div id="main" style="width: 100%;height:300px;margin:0 auto"></div>
      </el-col>
  </el-row>
  </div>
</template>
<script>
import { GetPlatTrade } from "@/api/user.js"
export default {
  data() {
    return {
      value6: "",
      listQuery: {
        begindate: this.cfg.formatDate(),
        enddate: this.cfg.formatDate(),
        keyword: ""
      },
      list:{},
    };
  },
  created(){
    this.getList();
  },
  methods: {
    getList(){
      GetPlatTrade("get",this.listQuery).then(response => {
        this.list = response.data;
        this.getcount_Chart();
        this.getamount_Chart();
        this.getcommission_Chart();
        this.getMain_Chart();
      });
    },
    getcount_Chart() {
      //交易详情总笔数
      // 基于准备好的dom，初始化echarts实例
      var option = {
        title: {
          text: "",
          subtext: "",
          x: "center"
        },
        tooltip: {
          trigger: "item",
          formatter: "{a} <br/>{b} : {c} ({d}%)"
        },
        legend: {
          orient: "vertical",
          left: "left",
          data: ["微信", "支付宝", "京东", "QQ钱包"]
        },
        series: [
          {
            name: "访问来源",
            type: "pie",
            radius: "55%",
            center: ["50%", "60%"],
            data: [
              {
                value: this.list.wechattradeqty,
                name: "微信",
                itemStyle: { normal: { color: "#2AA145" } }
              },
              {
                value: this.list.alipaytradeqty,
                name: "支付宝",
                itemStyle: { normal: { color: "#56ABE2" } }
              },
              {
                value: this.list.jdtradeqty,
                name: "京东",
                itemStyle: { normal: { color: "#E31D1A" } }
              },
              {
                value: this.list.qqtradeqty,
                name: "QQ钱包",
                itemStyle: { normal: { color: "#61A0A8" } }
              }
            ],
            itemStyle: {
              emphasis: {
                shadowBlur: 10,
                shadowOffsetX: 0,
                shadowColor: "rgba(0, 0, 0, 0.5)"
              }
            }
          }
        ]
      };
      var echarts_count = require("echarts");
      // 基于准备好的dom，初始化echarts实例
      var myChart = echarts_count.init(document.getElementById("sumcount"));
      // 使用刚指定的配置项和数据显示图表。
      myChart.setOption(option);
    },
    getamount_Chart() {
      //交易详情总金额
     // 基于准备好的dom，初始化echarts实例
      var option = {
        title: {
          text: "",
          subtext: "",
          x: "center"
        },
        tooltip: {
          trigger: "item",
          formatter: "{a} <br/>{b} : {c} ({d}%)"
        },
        legend: {
          orient: "vertical",
          left: "left",
          data: ["微信", "支付宝", "京东", "QQ钱包"]
        },
        series: [
          {
            name: "访问来源",
            type: "pie",
            radius: "55%",
            center: ["50%", "60%"],
            data: [
              {
                value: this.list.wechatamount,
                name: "微信",
                itemStyle: { normal: { color: "#2AA145" } }
              },
              {
                value: this.list.alipayamount,
                name: "支付宝",
                itemStyle: { normal: { color: "#56ABE2" } }
              },
              {
                value: this.list.jdamount,
                name: "京东",
                itemStyle: { normal: { color: "#E31D1A" } }
              },
              {
                value: this.list.qqamount,
                name: "QQ钱包",
                itemStyle: { normal: { color: "#61A0A8" } }
              }
            ],
            itemStyle: {
              emphasis: {
                shadowBlur: 10,
                shadowOffsetX: 0,
                shadowColor: "rgba(0, 0, 0, 0.5)"
              }
            }
          }
        ]
      };
      var echarts_amount = require("echarts");
      // 基于准备好的dom，初始化echarts实例
      var myChart = echarts_amount.init(document.getElementById("summoney"));
      // 使用刚指定的配置项和数据显示图表。
      myChart.setOption(option);
    },
    getcommission_Chart() {
      //交易详情总佣金
     // 基于准备好的dom，初始化echarts实例
      var option = {
        title: {
          text: "",
          subtext: "",
          x: "center"
        },
        tooltip: {
          trigger: "item",
          formatter: "{a} <br/>{b} : {c} ({d}%)"
        },
        legend: {
          orient: "vertical",
          left: "left",
          data: ["微信", "支付宝", "京东", "QQ钱包"]
        },
        series: [
          {
            name: "访问来源",
            type: "pie",
            radius: "55%",
            center: ["50%", "60%"],
            data: [
              {
                value: this.list.wechatcommission,
                name: "微信",
                itemStyle: { normal: { color: "#2AA145" } }
              },
              {
                value: this.list.alipaycommission,
                name: "支付宝",
                itemStyle: { normal: { color: "#56ABE2" } }
              },
              {
                value: this.list.jdcommission,
                name: "京东",
                itemStyle: { normal: { color: "#E31D1A" } }
              },
              {
                value: this.list.qqcommission,
                name: "QQ钱包",
                itemStyle: { normal: { color: "#61A0A8" } }
              }
            ],
            itemStyle: {
              emphasis: {
                shadowBlur: 10,
                shadowOffsetX: 0,
                shadowColor: "rgba(0, 0, 0, 0.5)"
              }
            }
          }
        ]
      };
      var echarts_amount = require("echarts");
      // 基于准备好的dom，初始化echarts实例
      var myChart = echarts_amount.init(document.getElementById("sumcommission"));
      // 使用刚指定的配置项和数据显示图表。
      myChart.setOption(option);
    },
    getMain_Chart(){
      // 基于准备好的dom，初始化echarts实例
      var option = {
          tooltip : {
              trigger: 'axis',
              axisPointer : {            // 坐标轴指示器，坐标轴触发有效
                  type : 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
              }
          },
          legend: {
              data:['平台成本','收益','分佣']
          },
          grid: {
              left: '3%',
              right: '4%',
              bottom: '3%',
              containLabel: true
          },
          xAxis : [
              {
                  type : 'category',
                  data : ['微信','支付宝','京东','QQ钱包']
              }
          ],
          yAxis : [
              {
                  type : 'value'
              }
          ],
          series : [
              {
                  name:'平台成本',
                  type:'bar',
                  data:[this.list.wechatplatfee, this.list.alipayplatfee, this.list.jdplatfee, this.list.qqplatfee],
              },
              {
                  name:'收益',
                  type:'bar',
                  stack: '广告',
                  data:[this.list.wechatprofit, this.list.alipayprofit, this.list.jdprofit, this.list.qqprofit]
              },
              {
                  name:'分佣',
                  type:'bar',
                  data:[this.list.wechatcommission, this.list.alipaycommission, this.list.jdcommission, this.list.qqcommission]
              }
          ]
      };

      var echarts_amount = require("echarts");
      // 基于准备好的dom，初始化echarts实例
      var myChart = echarts_amount.init(document.getElementById("main"));
      // 使用刚指定的配置项和数据显示图表。
      myChart.setOption(option);
    }
  }
};
</script> 
<style scoped>
.data-container {
  padding: 10px;
}
.grid {
  margin-top: 20px;
  border: 1px solid #ddd;
}
.grid-title {
  background: #eee;
  text-align: center;
  border-right: 1px solid #ddd;
  height: 50px;
  line-height: 50px;
  font-size: 22px;
}
.grid-title:last-child {
  border-right: 0 !important;
}
.grid-content {
  background: #fff;
  text-align: center;
  border-right: 1px solid #ddd;
  height: 100px;
  line-height: 100px;
  font-size: 22px;
}
.grid-echart {
  width: 100%;
  height: 200px;
  border-right: 1px solid #ddd;
  border-top: 1px solid #ddd;
  text-align: center;
}
</style>
