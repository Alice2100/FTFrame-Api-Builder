/*View code reference:
1、<NewCom_Mod ref="NewCom_Mod"/>
2、import NewCom_Mod from '@/ftdp/components/NewCom_Mod?'
3、components: { NewCom_Mod }
*/
/*Form component methods, properties, events:
Event callback:
	@beforeGet: The event before obtaining data from the interface, with optional parameters being the component object
	@beforeSet: an event that has not yet been assigned a value after obtaining data from the interface, with optional parameters being the component object
	@afterSet: Event after assignment, optional parameter is component object
	@beforeSubmit: Event before submission, optional parameter is component object
	@afterSubmit: Event after successful submission, optional parameter is component object
Retention methods and attributes:
	fill(paras): Load data, where params is the sequence parameter of the interface and the string after/in the URL
	reset(allEmpty): Reset. When the optional parameter is true, all are left blank
	submit(): Submit operation
	fileDownload(fileInfo): File Download
	get(apiPath, paras, callback): Get call
	post(apiPath, paras, json, callback): Post call
*/
<template>
	<el-form ref="form" :model="form" label-width="200px" label-position="left" :rules="formRules">
		<el-form-item label="Name" prop="name">
			<el-input clearable v-model="form.name">
			</el-input>
		</el-form-item>
		<el-form-item label="Phone Number" prop="phone">
			<el-input clearable v-model="form.phone">
			</el-input>
		</el-form-item>
		<el-form-item label="Sex" prop="sex">
			<el-input clearable v-model="form.sex">
			</el-input>
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
						<el-button type="primary" icon="el-icon-goods" @click="submit()">submit</el-button>
		</el-col></el-row>
	</el-form>
</template>

<script>
import { ftdpConfig } from '@/ftdp/utils/config.js'
import { ftdpBase } from '@/ftdp/utils/base.js'
//Config
var apiBase=ftdpConfig.apiBase;
var config={
	apiGet:apiBase+"/new_dir_1/new_page_10?get",
	apiSet:apiBase+"/new_dir_1/new_page_10?mod",
	tokenKey:ftdpConfig.tokenKey,
	tokenVal:ftdpConfig.getTokenLocal(),
}
//Data definition
var form={
	name:"",
	phone:"",
	sex:"",
	education:"",
	mimo:"",
	stat:{
		options:[{value:'1',label:'1'},{value:'2',label:'2'}],
		selValue:''
	},
}
var oriform='{}';
var inited = false;

export default  { 
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
form.sex = detail.sex; //Sex
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
json['sex'] = form.sex; //Sex
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
