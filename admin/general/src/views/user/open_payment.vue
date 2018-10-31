<template>
  <div class="app-container calendar-list-container">
    <div class="filter-container">
      <el-input @keyup.enter.native="handleFilter" style="width: 200px;" class="filter-item" placeholder="商户编码" v-model="listQuery.code">
      </el-input>
      <el-button class="filter-item" type="primary" icon="search" @click="handleFilter">搜索</el-button>
      <el-button class="filter-item" style="margin-left: 10px;" @click="handleCreate" type="primary" icon="edit">添加</el-button>
    </div>
    <el-table :data="list" v-loading.body="listLoading" element-loading-text="拼命加载中" border fit highlight-current-row> 
      <el-table-column align="center" label='序号' width="95">
        <template scope="scope">
          {{scope.$index}}
        </template>
      </el-table-column>
      <el-table-column label="代理商"  align="center">
        <template scope="scope">
          <span>{{scope.row.agname}}</span>
        </template>
      </el-table-column>
      <el-table-column label="大商户"  align="center" width="200">
        <template scope="scope">
          <span>{{scope.row.merchantname}}</span>
        </template>
      </el-table-column>
      <el-table-column label="商户名称"  align="center" width="200">
        <template scope="scope">
          <span>{{scope.row.shortName}}</span>
        </template>
      </el-table-column>
      <el-table-column label="保底"  align="center" width="100">
        <template scope="scope">
          <span>
            {{scope.row.futureMinAmount}}
            <!-- <el-input v-model="scope.row.futureMinAmount" :blur="FormChange()" placeholder="请输入内容"></el-input> -->
          </span>
        </template>
      </el-table-column>
      <el-table-column label="封顶"  align="center" width="200">
        <template scope="scope">
          <span>{{scope.row.futureMaxAmount}}</span>
        </template>
      </el-table-column>
      <el-table-column label="微信支付状态"  align="center" width="200">
        <template scope="scope">
          <!-- <span>{{scope.row.wechat_status}}</span> -->
          <el-button @click.prevent="handleUpdate(scope.row,'wechat')" type="text" size="small">{{scope.row.wechat_status|paystatusFilter}}</el-button>
        </template>
      </el-table-column>
      <el-table-column label="微信费率(%)"  align="center" width="200">
        <template scope="scope">
          <span>{{scope.row.wechat_rate}}</span>
        </template>
      </el-table-column>
      <el-table-column label="支付宝支付状态"  align="center" width="200">
        <template scope="scope">
          <!-- <span>{{scope.row.alipay_status}}</span> -->
          <el-button @click.prevent="handleUpdate(scope.row,'alipay')" type="text" size="small">{{scope.row.alipay_status|paystatusFilter}}</el-button>
        </template>
      </el-table-column>
      <el-table-column label="支付宝费率(%)"  align="center" width="200">
        <template scope="scope">
          <span>{{scope.row.alipay_rate}}</span>
        </template>
      </el-table-column>

      <el-table-column label="QQ支付状态"  align="center" width="200">
        <template scope="scope">
          <!-- <span>{{scope.row.alipay_status}}</span> -->
          <el-button @click.prevent="handleUpdate(scope.row,'qqpay')" type="text" size="small">{{scope.row.qqpay_status|paystatusFilter}}</el-button>
        </template>
      </el-table-column>
      <el-table-column label="QQ费率(%)"  align="center" width="200">
        <template scope="scope">
          <span>{{scope.row.qqpay_rate}}</span>
        </template>
      </el-table-column>
      <el-table-column label="京东支付状态"  align="center" width="200">
        <template scope="scope">
          <!-- <span>{{scope.row.alipay_status}}</span> -->
          <el-button @click.prevent="handleUpdate(scope.row,'jdpay')" type="text" size="small">{{scope.row.jdpay_status|paystatusFilter}}</el-button>
        </template>
      </el-table-column>
      <el-table-column label="京东费率(%)"  align="center" width="200">
        <template scope="scope">
          <span>{{scope.row.jdpay_rate}}</span>
        </template>
      </el-table-column>
      <!-- <el-table-column label="银联支付状态"  align="center" width="200">
        <template scope="scope">
          <el-button @click.prevent="handleUpdate(scope.row,'unionpay')" type="text" size="small">{{scope.row.unionpay_status|paystatusFilter}}</el-button>
        </template>
      </el-table-column>
      <el-table-column label="银联费率(%)"  align="center" width="200">
        <template scope="scope">
          <span>{{scope.row.unionpay_rate}}</span>
        </template>
      </el-table-column> -->


      <el-table-column label="代付支付状态"  align="center" width="200">
        <template scope="scope">
          <!-- <span>{{scope.row.daipay_status}}</span> -->
          <el-button @click.prevent="handleUpdate(scope.row,'daipay')" type="text" size="small">{{scope.row.daipay_status|paystatusFilter}}</el-button>
        </template>
      </el-table-column>
      <el-table-column label="代付单笔费用"  align="center" width="200">
        <template scope="scope">
          <span>{{scope.row.daipay_rate}}</span>
        </template>
      </el-table-column>
      <el-table-column label="审核状态"  align="center" width="200">
        <template scope="scope">
          <el-tag :type="scope.row.isaudit==true?'success':''">{{scope.row.isaudit==true?"已审核":"未审核"}}</el-tag>
        </template>
      </el-table-column>
      <el-table-column
        fixed="right"
        label="操作"
        width="100">
        <template scope="scope">
          <el-button @click.prevent="audit(scope.row)" type="primary" size="small" :disabled="scope.row.isaudit==true?true:false">审核</el-button>
        </template>
      </el-table-column>
    </el-table>

    <div v-show="!listLoading" class="pagination-container">
      <el-pagination @size-change="handleSizeChange" @current-change="handleCurrentChange" :current-page.sync="listQuery.page"
        :page-sizes="[10,20,30, 50]" :page-size="listQuery.pagesize" layout="total, sizes, prev, pager, next, jumper" :total="total">
      </el-pagination>
    </div>
    

    <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible" :close-on-click-modal="false">
      <el-form class="small-space" :model="ruleForm" :rules="rules" ref="ruleForm" label-position="left" label-width="150px" style='width: 620px; margin-left:50px;'>
        <el-form-item label="商户名称" prop="merchantId">
          <el-select v-model="ruleForm.merchantId" filterable placeholder="请选择商户" style="width:100%">
            <el-option
              v-for="item in business_basic"
              :key="item.appId"
              :label="item.shortName"
              :value="item.appId">
            </el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="操作类型" prop="handleType">
          <el-radio-group v-model="ruleForm.handleType">
          <el-radio label="0">新增</el-radio>
          <!-- <el-radio label="1">删除</el-radio> -->
        </el-radio-group>
        </el-form-item>
        <!-- <el-form-item label="结算周期" prop="cycleValue">
          <el-radio-group v-model="ruleForm.cycleValue">
          <el-radio label="0">T+0</el-radio>
          <el-radio label="1">T+1</el-radio>
          <el-radio label="2">D+0</el-radio>
          <el-radio label="3">D+1</el-radio>
        </el-radio-group>
        </el-form-item> -->
        <el-form-item label="保底" prop="futureMinAmount">
          <el-input v-model.number="ruleForm.futureMinAmount"></el-input>
        </el-form-item>
        <el-form-item label="封顶" prop="futureMaxAmount">
          <el-input v-model.number="ruleForm.futureMaxAmount"></el-input>
        </el-form-item>
        <el-form-item label="是否开通微信" prop="wechat_status">
          <el-radio v-model="ruleForm.wechat_status" :label="3" disabled>开通</el-radio>
        </el-form-item>
        <el-form-item label="微信费率(%)" prop="wechat_rate">
          <el-input v-model.number="ruleForm.wechat_rate" :disabled="ruleForm.wechat_status?false:true"></el-input>
        </el-form-item>
        <el-form-item label="是否开通支付宝" prop="alipay_status">
          <el-radio v-model="ruleForm.alipay_status" :label="3" disabled>开通</el-radio>
        </el-form-item>
        <el-form-item label="支付宝费率(%)" prop="alipay_rate">
          <el-input v-model.number="ruleForm.alipay_rate" :disabled="ruleForm.alipay_status?false:true"></el-input>
        </el-form-item>

        <el-form-item label="是否开通QQ钱包" prop="qqpay_status">
          <el-radio v-model="ruleForm.qqpay_status" :label="3" disabled>开通</el-radio>
        </el-form-item>
        <el-form-item label="QQ钱包费率(%)" prop="qqpay_rate">
          <el-input v-model.number="ruleForm.qqpay_rate" :disabled="ruleForm.qqpay_status?false:true"></el-input>
        </el-form-item>
        <el-form-item label="是否开通京东" prop="jdpay_status">
          <el-radio v-model="ruleForm.jdpay_status" :label="3" disabled>开通</el-radio>
        </el-form-item>
        <el-form-item label="京东费率(%)" prop="jdpay_rate">
          <el-input v-model.number="ruleForm.jdpay_rate" :disabled="ruleForm.jdpay_status?false:true"></el-input>
        </el-form-item>


        <el-form-item label="是否开通代付" prop="daipay_status">
          <el-radio v-model="ruleForm.daipay_status" :label="3" disabled>开通</el-radio>
        </el-form-item>
        <el-form-item label="代付单笔费用" prop="daipay_rate">
          <el-input v-model.number="ruleForm.daipay_rate" :disabled="ruleForm.daipay_status?false:true"></el-input>
        </el-form-item>
      </el-form>
      
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogFormVisible = false">取 消</el-button>
        <el-button v-if="dialogStatus=='create'" type="primary" @click="create('ruleForm')">确 定 </el-button>
        <el-button v-else type="primary" @click="update('ruleForm')">确 定</el-button>
      </div>
    </el-dialog>

    <el-dialog :title="textBusinessMap[dialogBusinessStatus]" :visible.sync="dialogBusinessVisible">
      <el-form class="small-space" :model="ruleForm" :rules="rules" ref="ruleForm" label-position="left" label-width="150px" style='width: 620px; margin-left:50px;'>
        <div v-show="dialogBusinessStatus=='wechat'">
          <el-form-item label="是否开通微信" prop="wechat_status">
            <el-radio v-model="ruleForm.wechat_status" :label="3">开通</el-radio>
            <el-radio v-model="ruleForm.wechat_status" :label="2">关闭</el-radio>
          </el-form-item>
          <el-form-item label="微信费率(%)" prop="wechat_rate">
            <el-input v-model.number="ruleForm.wechat_rate" :disabled="ruleForm.wechat_status==3?false:true"></el-input>
          </el-form-item>
        </div>
        <div v-show="dialogBusinessStatus=='alipay'">
          <el-form-item label="是否开通支付宝" prop="alipay_status">
            <el-radio v-model="ruleForm.alipay_status" :label="3">开通</el-radio>
            <el-radio v-model="ruleForm.alipay_status" :label="2">关闭</el-radio>
          </el-form-item>
          <el-form-item label="支付宝费率(%)" prop="alipay_rate">
            <el-input v-model.number="ruleForm.alipay_rate" :disabled="ruleForm.alipay_status==3?false:true"></el-input>
          </el-form-item>
        </div>
        <div v-show="dialogBusinessStatus=='daipay'">
          <el-form-item label="是否开通代付" prop="daipay_status">
            <el-radio v-model="ruleForm.daipay_status" :label="3">开通</el-radio>
            <el-radio v-model="ruleForm.daipay_status" :label="2">关闭</el-radio>
          </el-form-item>
          <el-form-item label="代付单笔费用" prop="daipay_rate">
            <el-input v-model.number="ruleForm.daipay_rate" :disabled="ruleForm.daipay_status==3?false:true"></el-input>
          </el-form-item>
        </div>

        <div v-show="dialogBusinessStatus=='qqpay'">
          <el-form-item label="是否开通QQ钱包" prop="qqpay_status">
            <el-radio v-model="ruleForm.qqpay_status" :label="3">开通</el-radio>
            <el-radio v-model="ruleForm.qqpay_status" :label="2">关闭</el-radio>
          </el-form-item>
          <el-form-item label="QQ费率(%)" prop="qqpay_rate">
            <el-input v-model.number="ruleForm.qqpay_rate" :disabled="ruleForm.qqpay_status==3?false:true"></el-input>
          </el-form-item>
        </div>

        <div v-show="dialogBusinessStatus=='jdpay'">
          <el-form-item label="是否开通京东" prop="jdpay_status">
            <el-radio v-model="ruleForm.jdpay_status" :label="3">开通</el-radio>
            <el-radio v-model="ruleForm.jdpay_status" :label="2">关闭</el-radio>
          </el-form-item>
          <el-form-item label="京东费率(%)" prop="jdpay_rate">
            <el-input v-model.number="ruleForm.jdpay_rate" :disabled="ruleForm.jdpay_status==3?false:true"></el-input>
          </el-form-item>
        </div>

        <div v-show="dialogBusinessStatus=='unionpay'">
          <el-form-item label="是否开通银联" prop="unionpay_status">
            <el-radio v-model="ruleForm.unionpay_status" :label="3">开通</el-radio>
            <el-radio v-model="ruleForm.unionpay_status" :label="2">关闭</el-radio>
          </el-form-item>
          <el-form-item label="银联费率(%)" prop="unionpay_rate">
            <el-input v-model.number="ruleForm.unionpay_rate" :disabled="ruleForm.unionpay_status==3?false:true"></el-input>
          </el-form-item>
        </div>
     
      </el-form>
      
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogBusinessVisible = false">取 消</el-button>
        <el-button type="primary" @click="update('ruleForm')">确 定</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import { businessbasic, openpayment, editopenpayment ,GetNotOpenPayment} from "@/api/user.js";
import { businesscoding } from "@/api/basic.js";
import { merchOpenBusiness } from "@/api/java_user.js";

export default {
  data() {
    var validatePass = (rule, value, callback) => {
      //密码
      if (value === "" && this.dialogStatus == "create") {
        callback(new Error("请输入密码"));
      } else {
        callback();
      }
    };
    var checkwechat_rate = (rule, value, callback) => {
      if (this.ruleForm.wechat_status) {
        if (!Number.parseFloat(value)) {
          callback(new Error("请输入数字值"));
        } else {
          if (value >= 10) {
            callback(new Error("费率不能大于10"));
          } else {
            callback();
          }
        }
      } else {
        callback();
      }
    };
    var checkalipay_rate = (rule, value, callback) => {
      if (this.ruleForm.alipay_status) {
        if (!Number.parseFloat(value)) {
          callback(new Error("请输入数字值"));
        } else {
          if (value >= 10) {
            callback(new Error("费率不能大于10"));
          } else {
            callback();
          }
        }
      } else {
        callback();
      }
    };
    var checkdaipay_rate = (rule, value, callback) => {
      if (this.ruleForm.daipay_status) {
        if (!Number.parseFloat(value)) {
          callback(new Error("请输入数字值"));
        } else {
          if (value >= 10) {
            callback(new Error("费率不能大于10"));
          } else {
            callback();
          }
        }
      } else {
        callback();
      }
    };
    var checkfutureMinAmount = (rule, value, callback) => {
      if (!value) {
        if (!Number.isInteger(value)) {
          callback(new Error("请输入数字值"));
        } else {
          callback();
        }
      } else {
        callback();
      }
    };
    var checkfutureMaxAmount = (rule, value, callback) => {
      if (!value) {
        if (!Number.isInteger(value)) {
          callback(new Error("请输入数字值"));
        } else {
          callback();
        }
      } else {
        callback();
      }
    };
    return {
      list: null,
      total: null,
      listLoading: true,
      listQuery: {
        page: 1,
        pagesize: this.basepagesize,
        code: "",
        merchantid: ""
      },
      business_category: [], //经营类目
      business_coding: [], //业务类型
      business_basic: [], //商户列表
      ruleForm: "",
      dialogFormVisible: false,
      dialogStatus: "",
      textMap: {
        update: "编辑",
        create: "创建"
      },
      dialogBusinessVisible: false,
      dialogBusinessStatus: "",
      textBusinessMap: {
        wechat: "微信",
        alipay: "支付宝",
        daipay: "代付",
        qqpay: "QQ",
        jdpay: "京东",
        unionpay: "银联"
      },
      dialogPvVisible: false,
      rules: {
        merchantId: [{ required: true, message: "请选择商户编号", trigger: "change" }],
        handleType: [{ required: true, message: "请输入操作类型", trigger: "blur" }],
        cycleValue: [{ required: true, message: "请输入结算周期", trigger: "blur" }],
        busiCode: [{ required: true, message: "请输入开通业务", trigger: "blur" }],
        futureRateType: [
          { required: true, message: "请输入费率类型", trigger: "blur" }
        ],
        futureMinAmount: [{ validator: checkfutureMinAmount, trigger: "blur" }],
        futureMaxAmount: [{ validator: checkfutureMaxAmount, trigger: "blur" }],
        wechat_rate: [{ validator: checkwechat_rate, trigger: "blur" }],
        alipay_rate: [{ validator: checkalipay_rate, trigger: "blur" }],
        jdpay_rate: [{ validator: checkalipay_rate, trigger: "blur" }],
        qqpay_rate: [{ validator: checkalipay_rate, trigger: "blur" }],
        daipay_rate: [{ validator: checkdaipay_rate, trigger: "blur" }]
      }
    };
  },
  computed: {},
  filters: {
    paystatusFilter(status) {
      const statusMap = {
        2: "关闭",
        3: "开通"
      };
      return statusMap[status];
    },
    dateFilter(value) {
      if (value) {
        return value.replace("T", " ");
      }
    }
  },
  created() {
    this.fetchData();
    this.getbasic();
  },
  methods: {
    resetTemp() {
      this.ruleForm = {
        id: "", //id
        merchantId: "", //商户编号
        handleType: "0", //操作类型
        cycleValue: "2", //结算周期
        busiCode: "B00107,B00110,B00109,B00112,B00126,B00123,B00302", //开通业务
        futureRateType: "1,1,1,1,1,1,2", //费率类型
        futureRateValue: "", //费率
        futureMinAmount: "0.01", //保底
        futureMaxAmount: "50000", //封顶
        agencyId: "", //代理商编号
        wechat_status: 3, //是否开通微信
        wechat_rate: "0.70", //微信费率
        alipay_status: 3, //是否开通支付宝
        alipay_rate: "0.70", //支付宝费率
        daipay_status: 3, //是否开通代付
        daipay_rate: "0.50", //代付费率
        qqpay_status: 3, //是否开通qq
        qqpay_rate: "0.70", //qq费率
        jdpay_status: 3, //是否开通jd
        jdpay_rate: "0.70", //jd费率
        unionpay_status: 3, //是否开通银联
        unionpay_rate: "0.70" //银联费率
      };
    },
    handleFilter() {
      this.listQuery.page = 1;
      this.fetchData();
    },
    handleCurrentChange(val) {
      this.listQuery.page = val;
      this.fetchData();
    },
    handleSizeChange(val) {
      this.listQuery.pagesize = val;
      this.getList();
    },
    handleCreate() {
      this.resetTemp();
      this.dialogStatus = "create";
      this.dialogFormVisible = true;
    },
    handleUpdate(row, status) {
      this.ruleForm = Object.assign({}, row);
      this.ruleForm.wechat_status = this.ruleForm.wechat_status;
      this.dialogBusinessStatus = status;
      this.ruleForm.handleType = "1";
      this.ruleForm.futureRateType = "1";
      this.dialogBusinessVisible = true;
    },
    // handleDelete(row) {
    //   this.$confirm("此操作删除商家, 是否继续?", "提示", {
    //     confirmButtonText: "确定",
    //     cancelButtonText: "取消",
    //     type: "warning"
    //   }).then(() => {
    //     this.ruleForm = Object.assign({}, row);
    //     businessbasic("delete", { id: this.ruleForm.id }).then(response => {
    //       this.dialogFormVisible = false;
    //       this.fetchData();
    //       this.$notify({
    //         title: "成功",
    //         message: "删除成功",
    //         type: "success",
    //         duration: 2000
    //       });
    //     });
    //   });
    // },
    fetchData() {
      this.listLoading = true;
      openpayment("get", this.listQuery).then(response => {
        this.list = response.data;
        this.total = response.total;

        this.listLoading = false;
      });
      // console.log(bData);
      // this.list = bData.router;
      // this.total = bData.router.length;
      // this.listLoading = false;
    },
    getbasic() {
      GetNotOpenPayment().then(response => {
        this.business_basic = response.data;
      });
      businesscoding("get").then(response => {
        this.business_coding = response.data;
      });
    },
    audit(row) {
      this.$confirm("此操作向渠道添加银行卡, 是否继续?", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning"
      }).then(() => {
        this.ruleForm = Object.assign({}, row);
        var formdata = Object.assign({}, this.ruleForm);
          
          //费率类型
          formdata.futureRateType="1,1,1,1,1,1,2", //费率类型
          //费率
          formdata.futureRateValue =
            formdata.wechat_rate +
            "," +
            formdata.wechat_rate +
            "," +
            formdata.alipay_rate +
            "," +
            formdata.alipay_rate +
            "," +
            formdata.qqpay_rate +
            "," +
            formdata.jdpay_rate +
            "," +
            formdata.daipay_rate;
          //保底
          formdata.futureMinAmount =
            formdata.futureMinAmount +
            "," +
            formdata.futureMinAmount +
            "," +
            formdata.futureMinAmount +
            "," +
            formdata.futureMinAmount +
            "," +
            formdata.futureMinAmount +
            "," +
            formdata.futureMinAmount +
            "," +
            formdata.futureMinAmount;
          //封顶
          formdata.futureMaxAmount =
            formdata.futureMaxAmount +
            "," +
            formdata.futureMaxAmount +
            "," +
            formdata.futureMaxAmount +
            "," +
            formdata.futureMaxAmount +
            "," +
            formdata.futureMaxAmount +
            "," +
            formdata.futureMaxAmount +
            "," +
            formdata.futureMaxAmount;
        this.$message({
          showClose: true,
          message: '请求发送成功，请等待后台返回状态后在进行操作！',
          type: 'success',
          duration: 5000
        });
        merchOpenBusiness("get", {
            data: formdata
          }).then(response => {
            if (response.data.returnState) {
              var resdata = response.data.data;
              if (resdata.status_code == 1) {
                this.ruleForm.isaudit=true;
                this.ruleForm.handleType = "1";
                editopenpayment("put", this.ruleForm).then(response => {
                 this.fetchData();
                });
                this.$notify({
                    title: "成功",
                    message: "审核成功",
                    type: "success",
                    duration: 0
                });
              } else {
                if(resdata.message.indexOf("重复") >= 0){//如果重复直接修改状态
                  this.ruleForm.isaudit=true;
                  this.ruleForm.handleType = "1";
                  editopenpayment("put", this.ruleForm).then(response => {
                     this.$notify({
                          title: "成功",
                          message: "审核成功",
                          type: "success",
                          duration: 3000
                     });
                     this.fetchData();
                  });
                }
                else{
                  this.$message.error(resdata.message);
                }
              }
            } else {
              this.$message.error(response.data.message);
            }
          });
      });
    },
    update(formName) {
      this.$refs[formName].validate(valid => {
        if (valid) {
          var formdata = Object.assign({}, this.ruleForm);
          console.log(this.dialogBusinessStatus);
          switch (this.dialogBusinessStatus) {
            case "wechat":
              formdata.busiCode = "B00107,B00110";
              formdata.futureRateValue =
                formdata.wechat_rate + "," + formdata.wechat_rate;
              formdata.futureRateType = "1,1";
              formdata.futureMinAmount =
                formdata.futureMinAmount + "," + formdata.futureMinAmount;
              // formdata.futureMinAmount="0.01,0.01";
              formdata.futureMaxAmount =
                formdata.futureMaxAmount + "," + formdata.futureMaxAmount;
              break;
            case "alipay":
              formdata.busiCode = "B00109,B00112";
              formdata.futureRateValue =
                formdata.alipay_rate + "," + formdata.alipay_rate;
              formdata.futureRateType = "1,1";
              formdata.futureMinAmount =
                formdata.futureMinAmount + "," + formdata.futureMinAmount;
              formdata.futureMaxAmount =
                formdata.futureMaxAmount + "," + formdata.futureMaxAmount;
              break;

            case "qqpay":
              formdata.busiCode = "B00126";
              formdata.futureRateValue = formdata.qqpay_rate;
              formdata.futureRateType = "1";
              formdata.futureMinAmount = formdata.futureMinAmount;
              formdata.futureMaxAmount = formdata.futureMaxAmount;
              break;
            case "jdpay":
              formdata.busiCode = "B00123";
              formdata.futureRateValue = formdata.jdpay_rate;
              formdata.futureRateType = "1";
              formdata.futureMinAmount = formdata.futureMinAmount;
              formdata.futureMaxAmount = formdata.futureMaxAmount;
              break;

            case "unionpay":
              formdata.busiCode = "B00121";
              formdata.futureRateValue = formdata.unionpay_rate;
              formdata.futureRateType = "1";
              formdata.futureMinAmount = formdata.futureMinAmount;
              formdata.futureMaxAmount = formdata.futureMaxAmount;
              break;

            case "daipay":
              formdata.futureRateValue = formdata.daipay_rate;
              formdata.futureRateType = "2";
              formdata.busiCode = "B00302";
              break;
          }
          merchOpenBusiness("get", {
            data: formdata
          }).then(response => {
            if (response.data.returnState) {
              var resdata = response.data.data;
              if (resdata.status_code == 1) {
                editopenpayment("put", this.ruleForm).then(response => {
                  this.dialogBusinessVisible = false;
                  this.fetchData();
                  this.$notify({
                    title: "成功",
                    message: "修改成功",
                    type: "success",
                    duration: 2000
                  });
                });
              } else {
                this.$message.error(resdata.message);
              }
            } else {
              this.$message.error(response.data.message);
            }
          });
        } else {
          console.log("error submit!!");
          return false;
        }
      });
    },
    create(formName) {
      this.$refs[formName].validate(valid => {
        if (valid) {
          editopenpayment("post", this.ruleForm).then(response => {
                this.dialogFormVisible = false;
                this.fetchData();
                this.$notify({
                title: "成功",
                message: "新增成功",
                type: "success",
                duration: 2000
           });
         });
        } else {
          console.log("error submit!!");
          return false;
        }
      });
    }
  },
  watch: {
    dialogFormVisible(val, oldValue) {
      if (!val) {
        this.$refs["ruleForm"].resetFields();
      }
    }
  }
};
</script>
<style scoped>
.filter-container {
  margin-bottom: 20px;
}
.link-type:hover,
.link-type:focus:hover {
  color: #20a0ff;
}
.link-type,
.link-type:focus {
  color: #337ab7;
  cursor: pointer;
}
.pagination-container {
  margin-top: 20px;
}
.el-tag + .el-tag {
  margin-left: 10px;
}
</style>
