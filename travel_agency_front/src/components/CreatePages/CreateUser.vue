<template>
    <div class="create-form">
        <label for="email">Email:</label>
        <input id="email" type="text" v-model="user.email" placeholder="Enter email" @input="checkCorrectInputs">
        <div class="error" v-if="!user.email">Email is required</div>

        <label for="role">Role:</label>
        <select id="role" v-model="user.role" @change="checkCorrectInputs">
            <option value="ADMIN">ADMIN</option>
            <option value="EDITOR">EDITOR</option>
        </select>
        <div class="error" v-if="!user.role">Role is required</div>

        <div v-if="message">{{ message }}</div>
        <button v-if="!isSendRequest" style="margin: 20px;" @click="createUser" :disabled="!isCorrectInputs">Add
            User</button>
        <button v-else style="background-color: #4CAF50; margin: 20px;" class="btn btn-primary" type="button"
            :disabled="!isCorrectInputs">
            <span class="spinner-border spinner-border-sm" aria-hidden="true"></span>
            <span role="status">Loading...</span>
        </button>
    </div>
</template>

<script>
import * as userAPI from '@/services/API/userAPI';
import * as validEmail from '@/js/validEmail';

export default {
    data() {
        return {
            user: {
                email: "",
                role: ""
            },
            isSendRequest: false,
            isCorrectInputs: false,
            message: ""
        }
    },

    methods: {
        async checkCorrectInputs() {
            this.isCorrectInputs = await validEmail.isValidEmail(this.user.email) && this.user.role;
        },


        async clearFields() {
            this.user.email = "";
            this.user.role = "";
            await this.checkCorrectInputs();
        },

        async createUser() {
            this.message = "";
            const token = localStorage.getItem('token');
            if (this.isCorrectInputs && token) {
                this.isSendRequest = true;
                const result = await userAPI.createUser(this.user, token);
                if (result === false) {
                    this.message = "User wasn't created"
                    this.isSendRequest = false;
                } else {
                    await this.clearFields();

                    this.isCorrectInputs = false;
                    this.isSendRequest = false;
                }
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