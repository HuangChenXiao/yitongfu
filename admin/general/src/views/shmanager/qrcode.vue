<template>
    <div class="app-container calendar-list-container">
        <div class="filter-container">
            <el-input @keyup.enter.native="handleFilter" style="width: 200px;" class="filter-item" placeholder="名称" v-model="listQuery.code">
            </el-input>

            <el-button class="filter-item" type="primary" icon="search" @click="handleFilter">搜索</el-button>
            <el-button class="filter-item" style="margin-left: 10px;" @click="handleCreate" type="primary" icon="edit">添加</el-button>
        </div>
        <el-table :data="list" v-loading.body="listLoading" element-loading-text="拼命加载中" border fit highlight-current-row>
            <el-table-column align="center" label='序号' width="95">
                <template scope="scope">
                    {{scope.$index+1}}
                </template>
            </el-table-column>
            <el-table-column label="二维码" align="center">
                <template scope="scope">
                    <a :href="scope.row.qrcode" target="_blank">
                        <img :src="scope.row.qrcode" width="40" height="40" alt="">
                    </a>
                </template>
            </el-table-column>
            <el-table-column label="名称">
                <template scope="scope">
                    <span class="link-type" @click="handleUpdate(scope.row)">{{scope.row.name}}</span>
                </template>
            </el-table-column>
            <el-table-column label="金额" align="center">
                <template scope="scope">
                    <span>{{scope.row.amount}}</span>
                </template>
            </el-table-column>
            <el-table-column label="类型" align="center">
                <template scope="scope">
                    <el-tag :type="'success'">{{scope.row.type==1?'微信':'支付宝'}}</el-tag>
                </template>
            </el-table-column>
            <el-table-column label="状态" align="center" width="100">
                <template scope="scope">
                    <el-tag :type="scope.row.status==1?'success':''">{{scope.row.status|statusFilter}}</el-tag>
                </template>
            </el-table-column>
            <el-table-column label="日期" align="center">
                <template scope="scope">
                    <span>{{scope.row.addtime|dateFilter}}</span>
                </template>
            </el-table-column>
            <el-table-column fixed="right" label="操作" width="150">
                <template scope="scope">
                    <el-button @click.prevent="handleUpdate(scope.row)" type="text" size="small">编辑</el-button>
                    <el-button @click.prevent="handleDelete(scope.row)" type="text" size="small">删除</el-button>
                </template>
            </el-table-column>
        </el-table>

        <div v-show="!listLoading" class="pagination-container">
            <el-pagination @size-change="handleSizeChange" @current-change="handleCurrentChange" :current-page.sync="listQuery.page" :page-sizes="[10,20,30, 50]" :page-size="listQuery.pagesize" layout="total, sizes, prev, pager, next, jumper" :total="total">
            </el-pagination>
        </div>

        <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible" :close-on-click-modal="false">
            <el-form :model="ruleForm" :rules="rules" ref="ruleForm" label-position="left" label-width="150px" style='width: 620px; margin-left:50px;'>

                <el-form-item label="二维码" prop="qrcode">
                    <el-upload class="avatar-uploader" :action="baseurl+'Upload'" :show-file-list="false" :on-success="handleAvatarSuccess" :before-upload="beforeAvatarUpload" :headers="{'Authorization':agToken}">
                        <img v-if="ruleForm.qrcode" :src="ruleForm.qrcode" class="avatar">
                        <i v-else class="el-icon-plus avatar-uploader-icon"></i>
                    </el-upload>
                </el-form-item>
                <el-form-item label="名称" prop="name">
                    <el-input v-model="ruleForm.name"></el-input>
                </el-form-item>
                <el-form-item label="金额" prop="amount">
                    <el-input v-model="ruleForm.amount"></el-input>
                </el-form-item>
                <el-form-item label="类型">
                    <el-radio-group v-model="ruleForm.type">
                        <el-radio :label="1">微信</el-radio>
                        <el-radio :label="2">支付宝</el-radio>
                    </el-radio-group>
                </el-form-item>
                <el-form-item label="状态">
                    <el-radio-group v-model="ruleForm.status">
                        <el-radio :label="0">禁用</el-radio>
                        <el-radio :label="1">启用</el-radio>
                    </el-radio-group>
                </el-form-item>
            </el-form>

            <div slot="footer" class="dialog-footer">
                <el-button @click="dialogFormVisible = false">取 消</el-button>
                <el-button v-if="dialogStatus=='create'" type="primary" @click="create('ruleForm')">确 定 </el-button>
                <el-button v-else type="primary" @click="update('ruleForm')">确 定</el-button>
            </div>
        </el-dialog>
    </div>
</template>

<script>
import { QrCode, EditQrCode } from '@/api/shmanager.js'
import { getToken } from '@/utils/auth'
import { validatenumber } from '@/utils/validate'
import md5 from 'js-md5'

export default {
  data() {
    var validatePass = (rule, value, callback) => {
      if (value === '' && this.dialogStatus == 'create') {
        callback(new Error('请输入密码'))
      } else {
        callback()
      }
    }
    var validatePass2 = (rule, value, callback) => {
      if (value === '') {
        callback(new Error('请再次输入密码'))
      } else if (value !== this.ruleForm.password) {
        callback(new Error('两次输入密码不一致!'))
      } else {
        callback()
      }
    }
    var ckh_proof_document = (rule, value, callback) => {
      if (value.length <= 0) {
        callback(new Error('请上传合同等必要的证明文件的扫描'))
      } else {
        callback()
      }
    }
    var ckh_agent_rights = (rule, value, callback) => {
      if (value.length <= 0) {
        callback(new Error('请选择代理商权限'))
      } else {
        callback()
      }
    }
    var checkratio = (rule, value, callback) => {
      if (!validatenumber(value)) {
        callback(new Error('请输入数值类型'))
      } else {
        callback()
      }
    }
    return {
      agToken: getToken(),
      list: null,
      total: null,
      listLoading: true,
      listQuery: {
        page: 1,
        pagesize: this.basepagesize,
        code: ''
      },
      role: '',
      rolelist: [],
      getrolelist: [],
      ruleForm: {},
      dialogFormVisible: false,
      dialogStatus: '',
      textMap: {
        update: '编辑',
        create: '创建'
      },
      dialogImageUrl: '',
      dialogVisible: false,
      dialogPvVisible: false,
      pvData: [],
      rules: {
        qrcode: [{ required: true, message: '请选择二维码', trigger: 'blur' }],
        name: [{ required: true, message: '请输入名称', trigger: 'blur' }],
        amount: [{ validator: checkratio, trigger: 'blur' }]
      }
    }
  },
  filters: {
    statusFilter(status) {
      const statusMap = {
        0: '禁用',
        1: '启用'
      }
      return statusMap[status]
    },
    dateFilter(value) {
      if (value) {
        return value.replace('T', ' ')
      }
    }
  },
  created() {
    this.fetchData()
  },
  methods: {
    handleFilter() {
      this.listQuery.page = 1
      this.fetchData()
    },
    handleCurrentChange(val) {
      this.listQuery.page = val
      this.fetchData()
    },
    handleSizeChange(val) {
      this.listQuery.pagesize = val
      this.getList()
    },
    handleCreate() {
      this.resetTemp()
      this.dialogStatus = 'create'
      this.dialogFormVisible = true
    },
    handleUpdate(row) {
      this.ruleForm = Object.assign({}, row)
      this.dialogStatus = 'update'
      this.dialogFormVisible = true
    },
    handleDelete(row) {
      this.$confirm('此操作删除该路由, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        this.ruleForm = Object.assign({}, row)
        QrCode('delete', { id: this.ruleForm.id }).then(response => {
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
    },
    fetchData() {
      this.listLoading = true
      QrCode('get', this.listQuery).then(response => {
        this.list = response.data
        this.total = response.total

        this.listLoading = false
      })
    },
    resetTemp() {
      this.ruleForm = {
        id: null,
        qrcode: null,
        amount: null,
        name: null,
        type: 1,
        status: 1
      }
    },
    update(formName) {
      this.$refs[formName].validate(valid => {
        if (valid) {
          EditQrCode('put', this.ruleForm).then(response => {
            this.dialogFormVisible = false
            this.fetchData()
            this.$notify({
              title: '成功',
              message: '修改成功',
              type: 'success',
              duration: 2000
            })
          })
        } else {
          console.log('error submit!!')
          return false
        }
      })
    },
    create(formName) {
      this.$refs[formName].validate(valid => {
        if (valid) {
          EditQrCode('post', this.ruleForm).then(response => {
            this.dialogFormVisible = false
            this.fetchData()
            this.$notify({
              title: '成功',
              message: '新增成功',
              type: 'success',
              duration: 2000
            })
          })
        } else {
          console.log('error submit!!')
          return false
        }
      })
    },
    handleRemove(file, fileList) {
      if (fileList.length > 0) {
        var imglist = []
        for (var i = 0; i < fileList.length; i++) {
          imglist.push(fileList[i].url)
        }
        this.ruleForm.proof_document = imglist
      } else {
        this.ruleForm.proof_document = []
      }
    },
    handlePictureCardPreview(file) {
      this.ruleForm.proof_document = file.response.url
      this.dialogVisible = true
    },
    handleListAvatarSuccess(res, file) {
      if (typeof this.ruleForm.proof_document == 'object') {
        this.ruleForm.proof_document.push(res.url)
      } else {
        if (this.ruleForm.proof_document) {
          var imglist = this.ruleForm.proof_document.split(',')
          imglist.push(res.url)
          this.ruleForm.proof_document = imglist
        }
      }
    },
    handleAvatarSuccess(res, file) {
      this.ruleForm.qrcode = res.url
    },
    beforeAvatarUpload(file) {
      const isJPG = file.type === 'image/jpeg' || file.type === 'image/png'
      const isLt2M = file.size / 1024 / 1024 < 2

      if (!isJPG) {
        this.$message.error('上传头像图片只能是 JPG 格式!')
      }
      if (!isLt2M) {
        this.$message.error('上传头像图片大小不能超过 2MB!')
      }
      return isJPG && isLt2M
    }
  },
  watch: {
    dialogFormVisible(val, oldValue) {
      if (!val) {
        this.$refs['ruleForm'].resetFields()
      }
    }
  }
}
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

<style>
.avatar-uploader .el-upload {
  border: 1px dashed #c0ccda;
  border-radius: 6px;
  cursor: pointer;
  position: relative;
  overflow: hidden;
}
.avatar-uploader .el-upload:hover {
  border-color: #409eff;
}
.avatar-uploader-icon {
  font-size: 28px;
  color: #8c939d;
  width: 148px;
  height: 148px;
  line-height: 148px;
  text-align: center;
}
.avatar {
  width: 148px;
  height: 148px;
  display: block;
}
</style>
