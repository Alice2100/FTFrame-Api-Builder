<template>
	<el-form ref="form" :model="form" label-width="180px" :rules="formRules">
		<el-form-item label="Name" prop="name">
			<el-input clearable v-model="form.name">
			</el-input>
		</el-form-item>
		<el-form-item label="Phone Number" prop="phone">
			<el-input clearable v-model="form.phone">
			</el-input>
		</el-form-item>
		<el-form-item label="Sex" prop="sex.selValue">
			<el-select clearable v-model="form.sex.selValue">
				<el-option v-for="item in form.sex.options" :key="item.value" :label="item.label" :value="item.value"></el-option>
			</el-select>
		</el-form-item>
		<el-form-item label="Education Background" prop="education">
			<el-input clearable v-model="form.education">
			</el-input>
		</el-form-item>
		<el-form-item label="notes" prop="mimo">
			<el-input clearable v-model="form.mimo">
			</el-input>
		</el-form-item>
		<el-form-item label="State" prop="stat.selValue">
			<el-select clearable v-model="form.stat.selValue">
				<el-option v-for="item in form.stat.options" :key="item.value" :label="item.label" :value="item.value"></el-option>
			</el-select>
		</el-form-item>
		<el-row><el-col style="text-align:center">
						<el-button type="primary" icon="el-icon-check" @click="submit()">Submit</el-button>
			<el-button type="info" icon="el-icon-info" @click="reset()">Reset</el-button>
		</el-col></el-row>
	</el-form>
</template>

<script>
//Config
var apiBase=ftdpConfig.apiBase;
var config={
	apiGet:apiBase+"/new_page_1?get",
	apiSet:apiBase+"/new_page_1?mod",
	tokenKey:ftdpConfig.tokenKey,
	tokenVal:ftdpConfig.getTokenLocal(),
}
//Data definition
var form={
	name:"",
	phone:"",
	sex:{
		options:[],
		selValue:''
	},
	education:"",
	mimo:"",
	stat:{
		options:[],
		selValue:''
	},
}
var oriform='{}';
var inited = false;

module.exports = { 
    data: function() {
        return {
			form:form,
			page: { loading: false },
            ftdpConfig: ftdpConfig,
            fillPara:'',
formRules: {
},
        }
    },
	methods: {
		fill(paras) {
			form_paras=paras;
			ftdpBase.form_fill(config,this,form_paras,(obj)=>{form_fill_json(obj,this);oriform=JSON.stringify(form);},()=>{return js_beforeget(this)},(resData)=>{return js_beforeset(this,resData)},(resData)=>{js_afterset(this,resData)});
		},
		reset(allEmpty) {
    this.$refs.form.resetFields()
      ftdpBase.form_reset(form, oriform, allEmpty)
    },
        func(f) {
        const Fn = Function
        new Fn(f + '(this)')()
        },
		submit() {
this.$refs.form.validate((valid) => {
          if (valid) {
            if(submit_before(this))ftdpBase.form_submit(config,this,form_paras,form_submit_json(this),()=>{submit_after(this)});
          } else {
            return false;
          }
        });
		},
get(apiPath, paras, callback) {
      ftdpBase.normal_get(this, ftdpConfig, apiPath, paras, callback)
    },
    post(apiPath, paras, json, callback) {
      ftdpBase.normal_post(this, ftdpConfig, apiPath, paras, json, callback)
    },
fileDownload(fileInfo) {
        ftdpBase.file_download(this, ftdpConfig, fileInfo)
    }
   },
	mounted:function(){
		if (!inited)init(this,()=>{oriform=JSON.stringify(form)});
        inited = true
	}
}
var form_paras='';
//Initialize data definition
function init(vm,callback)
{
	callback();
}
//Form Fill Definition
function form_fill_json(detail,vm)
{
/* Auto Generate */
form.name = detail.name; //Name
form.phone = detail.phone; //Phone Number
form.sex.selValue = detail.sex; //Sex
form.education = detail.education; //Education Background
form.mimo = detail.mimo; //notes
form.stat.selValue = detail.stat; //State

}
//Form Submission Definition
function form_submit_json(vm)
{
var json={};
/* Auto Generate */
json['name'] = form.name; //Name
json['phone'] = form.phone; //Phone Number
json['sex'] = form.sex.selValue; //Sex
json['education'] = form.education; //Education Background
json['mimo'] = form.mimo; //notes
json['stat'] = form.stat.selValue; //State

return json;
}
//Load pre script, return false to block
function js_beforeget(vm)
{
vm.$emit('beforeGet', vm);
return true;
}
//Script before assignment, return false to block
function js_beforeset(vm,resData)
{
//resData is json object return from Api
vm.$emit('beforeSet', vm)
return true;
}
//Script after assignment
function js_afterset(vm,resData)
{
//resData is json object return from Api
vm.$emit('afterSet', vm);
}
//Pre submission script, return false to block
function submit_before(vm)
{
vm.$emit('beforeSubmit', vm);
return true;
}
//Script after submission
function submit_after(vm)
{
vm.$emit('afterSubmit', vm)
}
</script>
<style scoped>

</style>
