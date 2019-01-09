<template>
  <div class="app-container calendar-list-container">
    <div class="filter-container">
      <el-input @keyup.enter.native="handleFilter" style="width: 200px;" class="filter-item" placeholder="模糊搜索" v-model="listQuery.code">
      </el-input>
      <el-select style="width: 200px;" class="filter-item" v-model="listQuery.enable">
        <el-option label="全部" value=""></el-option>
        <el-option label="启用" :value="true"></el-option>
        <el-option label="禁用" :value="false"></el-option>
      </el-select>
      <el-button class="filter-item" type="primary" icon="search" @click="handleFilter">搜索</el-button>
      <el-button class="filter-item" style="margin-left: 10px;" @click="handleCreate" type="primary" icon="edit">添加</el-button>
    </div>
    <el-table :data="list" v-loading.body="listLoading" element-loading-text="拼命加载中" border fit highlight-current-row> 
      <el-table-column align="center" label='序号' width="95">
        <template scope="scope">
          {{scope.$index+1}}
        </template>
      </el-table-column>
      <el-table-column label="支付宝账号"  align="center">
        <template scope="scope">
          <span class="link-type" @click="handleUpdate(scope.row)">{{scope.row.phone}}</span>
        </template>
      </el-table-column>
      <el-table-column align="center" label="支付宝ID">
        <template scope="scope">
        <span>{{scope.row.userid}}</span>
        </template>
      </el-table-column>
      <el-table-column align="center" label="代理商">
        <template scope="scope">
        <span>{{scope.row.agent_name}}</span>
        </template>
      </el-table-column>
      <el-table-column align="center" label="商户名称">
        <template scope="scope">
        <span>{{scope.row.shortName}}</span>
        </template>
      </el-table-column>
      <el-table-column label="支付宝"  align="center">
        <el-table-column align="center" label="名称" width="100">
            <template scope="scope">
            <span>{{scope.row.alipayname}}</span>
            </template>
        </el-table-column>
        <el-table-column align="center" label="是否开通" width="100">
            <template scope="scope">
            <span>{{scope.row.isalipay?'是':'否'}}</span>
            </template>
        </el-table-column>
        <el-table-column align="center" label="支付上限金额" width="120">
            <template scope="scope">
            <span>{{scope.row.alipayamount}}</span>
            </template>
        </el-table-column>
        <el-table-column align="center" label="已用金额" width="120">
            <template scope="scope">
            <span>{{scope.row.alipayuseamount}}</span>
            </template>
        </el-table-column>
      </el-table-column>
      <!-- <el-table-column label="微信"  align="center">
        <el-table-column align="center" label="名称" width="100">
            <template scope="scope">
            <span>{{scope.row.wechatname}}</span>
            </template>
        </el-table-column>
        <el-table-column align="center" label="是否开通" width="100">
            <template scope="scope">
            <span>{{scope.row.iswechat?'是':'否'}}</span>
            </template>
        </el-table-column>
        <el-table-column align="center" label="支付上限金额" width="120">
            <template scope="scope">
            <span>{{scope.row.wechatamount}}</span>
            </template>
        </el-table-column>
        <el-table-column align="center" label="已用金额" width="120">
            <template scope="scope">
            <span>{{scope.row.wechatuseamount}}</span>
            </template>
        </el-table-column>
      </el-table-column> -->
        <el-table-column align="center" label="是否可用" width="100">
            <template scope="scope">
            <el-tag :type="scope.row.enable?'success':'info'">{{scope.row.enable?'启用':'禁用'}}</el-tag>
            </template>
        </el-table-column>
      <el-table-column label="操作" width="180">
        <template scope="scope">
          <!-- <el-button @click.prevent="handleEnable(scope.row)" type="text" size="small">{{scope.row.enable?"禁用":"启用"}}</el-button> -->
          <el-button @click.prevent="handleUpdate(scope.row)" type="text" size="small">编辑</el-button>
          <el-button @click.prevent="handleDelete(scope.row)" type="text" size="small">删除</el-button>
        </template>
      </el-table-column>
    </el-table>

    <div v-show="!listLoading" class="pagination-container">
      <el-pagination @size-change="handleSizeChange" @current-change="handleCurrentChange" :current-page.sync="listQuery.page"
        :page-sizes="[10,20,30, 50]" :page-size="listQuery.pagesize" layout="total, sizes, prev, pager, next, jumper" :total="total">
      </el-pagination>
    </div>

    <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible">
      <el-form class="small-space" :model="ruleForm" :rules="rules" ref="ruleForm"  label-position="right" label-width="100px" style='margin:0 50px;'>
        <el-form-item label="商户名称">
          <el-select v-model="ruleForm.businessid" filterable placeholder="请选择商户名称"  style="width:100%">
          <el-option
            v-for="item in bus_list"
            :key="item.id"
            :label="item.shortName"
            :value="item.id">
          </el-option>
        </el-select>
        </el-form-item>
        <el-form-item label="支付宝账号" prop="phone">
          <el-input v-model="ruleForm.phone"></el-input>
        </el-form-item>
        <!-- <el-form-item label="用户ID" prop="userid">
          <el-input v-model="ruleForm.userid" :disabled="dialogStatus=='update'"></el-input>
        </el-form-item> -->
        <el-form-item label="支付宝名称" prop="alipayname">
          <el-input v-model="ruleForm.alipayname"></el-input>
        </el-form-item>
        <!-- <el-form-item label="微信名称" prop="wechatname">
          <el-input v-model="ruleForm.wechatname"></el-input>
        </el-form-item>
        <el-form-item label="微信是否开通" prop="iswechat">
          <el-radio :label="false" v-model="ruleForm.iswechat">未开通</el-radio>
          <el-radio :label="true" v-model="ruleForm.iswechat">已开通</el-radio>
        </el-form-item> -->
        <!-- <el-form-item label="支付宝是否开通" prop="isalipay">
          <el-radio :label="false" v-model="ruleForm.isalipay">未开通</el-radio>
          <el-radio :label="true" v-model="ruleForm.isalipay">已开通</el-radio>
        </el-form-item> -->
        <el-form-item label="支付宝支付上限金额" prop="alipayamount">
          <el-input v-model="ruleForm.alipayamount"></el-input>
        </el-form-item>
        <!-- <el-form-item label="微信支付上限金额" prop="wechatamount">
          <el-input v-model="ruleForm.wechatamount"></el-input>
        </el-form-item> -->
        <el-form-item label="是否启用">
          <el-radio :label="false" v-model="ruleForm.enable">禁用</el-radio>
          <el-radio :label="true" v-model="ruleForm.enable">启用</el-radio>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogFormVisible = false">取 消</el-button>
        <el-button v-if="dialogStatus=='create'" type="primary" @click="create('ruleForm')">确 定</el-button>
        <el-button v-else type="primary" @click="update('ruleForm')">确 定</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import { MeMember, EditMeMember, AgentBusinessBasinc } from "@/api/merchant.js";
import { validatenumber } from '@/utils/validate.js'
export default {
  data() {
    var chk_number = (rule, value, callback) => {
      if (!validatenumber(value)) {
        callback(new Error('请输入数值类型'))
      } else {
        callback()
      }
    }
    var chk_index = (rule, value, callback) => {
      if (!value) {
        callback(new Error('请选择项'))
      } else {
        callback()
      }
    }
    return {
      list: null,
      total: null,
      bus_list: null,
      listLoading: true,
      listQuery: {
        page: 1,
        pagesize: 10,
        agentid:this.$store.getters.userid,
        code: "",
        enable:null
      },
      ruleForm: {
        userid: "",
        roleid: null,
        usercode: "",
        username: "",
        password: "",
      },
      dialogFormVisible: false,
      dialogStatus: "",
      textMap: {
        update: "编辑",
        create: "创建"
      },
      dialogPvVisible: false,
      pvData: [],
      rules: {
        phone: [{ required: true, message: '请输入支付宝账号', trigger: 'blur' }],
        alipayname: [{ required: true, message: '请输入支付宝名称', trigger: 'blur' }],
        wechatname: [{ required: true, message: '请输入微信名称', trigger: 'blur' }],
        businessid: [{ required: true, message: '请输入商户名称', trigger: 'blur' }],
        userid: [{ required: true, message: '请输入用户ID', trigger: 'blur' }],
        alipayamount: [{ validator: chk_number, trigger: 'blur' }],
        wechatamount: [{ validator: chk_number, trigger: 'blur' }],
        businessid: [{ validator: chk_index, trigger: 'change' }]
      }
    };
  },
  filters: {
    statusFilter(status) {
      const statusMap = {
        1: "启用",
        2: "禁用"
      };
      return statusMap[status];
    }
  },
  created() {
    this.BusList();
    this.fetchData();
  },
  methods: {
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
    handleUpdate(row) {
      this.ruleForm = Object.assign({}, row);
      this.ruleForm.password = "";
      this.dialogStatus = "update";
      this.dialogFormVisible = true;
    },
    resetTemp() {
      this.ruleForm = {
        memberid: null,
        phone: null,
        alipayname: null,
        wechatname: null,
        iswechat: true,
        isalipay: true,
        alipayamount: null,
        alipayuseamount: null,
        wechatamount: null,
        wechatuseamount: null,
        enable: true,
        businessid: null,
        userid: null,
      };
    },
    fetchData() {
      this.listLoading = true;
      MeMember("get", this.listQuery).then(response => {
        this.list = response.data;
        this.total = response.data.length > 0 ? response.data.length : 0;

        this.listLoading = false;
      });
    },
    BusList() {
      AgentBusinessBasinc("get", {
        page: 1,
        pagesize: 9999,
        code: ''
      }).then(response => {
        this.bus_list = response.data;
      });
    },
    handleDelete(row) {
      this.$confirm('此操作删除该路由, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      })
        .then(() => {
          this.ruleForm = Object.assign({}, row)
          MeMember('delete', { autoid: this.ruleForm.memberid }).then(response => {
            this.dialogFormVisible = false
            this.fetchData()
            this.$notify({
              title: '成功',
              message: '删除成功',
              type: 'success',
              duration: 2000
            })
          })
        })
        .catch(() => {
          this.$message({
            type: 'info',
            message: '已取消删除'
          })
        })
    },
    handleEnable(row) {
      this.ruleForm = Object.assign({}, row);
      this.ruleForm.enable= !this.ruleForm.enable
      EditMeMember("put", this.ruleForm).then(response => {
        this.dialogFormVisible = false;
        this.fetchData();
        this.$notify({
          title: "成功",
          message: "操作成功",
          type: "success",
          duration: 2000
        });
      });
    },
    update(formName) {
      this.$refs[formName].validate(valid => {
        if (valid) {
          EditMeMember("put", this.ruleForm)
            .then(response => {
              this.dialogFormVisible = false;
              this.fetchData();
              this.$notify({
                title: "成功",
                message: "更新成功",
                type: "success",
                duration: 2000
              });
            })
        } else {
          console.log("error submit!!");
          return false;
        }
      });
    },
    create(formName) {
      this.$refs[formName].validate(valid => {
        if (valid) {
          EditMeMember("post", this.ruleForm)
            .then(response => {
              this.dialogFormVisible = false;
              this.fetchData();
              this.$notify({
                title: "成功",
                message: "新增成功",
                type: "success",
                duration: 2000
              });
            })
        } else {
          console.log("error submit!!");
          return false;
        }
      });
    },
    handleSelectionChange(val) {
      this.multipleSelection = val;
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
</style>
