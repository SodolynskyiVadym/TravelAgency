<template>
    <div class="create-form">
        <label for="email">Email:</label>
        <input id="email" type="text" v-model="user.email" placeholder="Enter email" @input="checkCorrectInputs">
        <div class="error" v-if="!user.email">Email is required</div>

        <label for="role">Role:</label>
        <input id="role" type="text" v-model="user.role" placeholder="Enter role" @input="checkCorrectInputs">
        <div class="error" v-if="!user.role">Role is required</div>

        <button v-if="!isSendRequest" style="margin: 20px;" @click="createUser" :disabled="!isCorrectInputs">Add
            User</button>
        <button v-else style="background-color: #4CAF50; margin: 20px;" class="btn btn-primary" type="button" :disabled="!isCorrectInputs">
            <span class="spinner-border spinner-border-sm" aria-hidden="true"></span>
            <span role="status">Loading...</span>
        </button>
    </div>
</template>

<script>
import * as userAPI from '@/services/API/userAPI';

export default {
    data() {
        return {
            user: {
                email: "",
                role: ""
            },
            isSendRequest: false,
            isCorrectInputs: false
        }
    },

    methods: {
        async checkCorrectInputs() {
            this.isCorrectInputs = this.user.email && this.user.role;
            console.log(this.isCorrectInputs);
        },


        async clearFields() {
            this.user.email = "";
            this.user.role = "";
            await this.checkCorrectInputs();
        },

        async createUser() {
            const token = localStorage.getItem('token');
            if (this.isCorrectInputs && token) {
                this.isSendRequest = true;
                await userAPI.createUser(this.user, token);
                await this.clearFields();

                this.isCorrectInputs = false;
                this.isSendRequest = false;
            }
        }
    },

    async mounted() {
        // this.usersEmails = await userAPI.getUsersEmals();
    }
}

</script>

<style>
@import "./../../assets/css/styleFormCreate.css";
</style>