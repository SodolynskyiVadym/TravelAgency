<template>
    <div style="text-align: center;">
        <h1>Users</h1>
        <button class="button-create-action" @click="enterUserCreatePage">Add user</button>

        <div class="form-control">
            <input class="input input-alt" placeholder="Type email of user" type="text" v-model="inputUserEmail"
                @input="searchUser">
            <span class="input-border input-border-alt"></span>
        </div>
        <div class="error-search" v-if="searchedUsers.length === 0">Incorrect user email</div>


        <table class="list-table">
            <tr>
                <th>Id</th>
                <th>Email</th>
                <th>Role</th>
                <th style="width: 400px;">Action</th>
            </tr>
            <tr v-for="user in searchedUsers" :key="user.id">
                <td>{{ user.id }}</td>
                <td>{{ user.email }}</td>
                <td>{{ user.role }}</td>
                <td>
                    <button @click="deleteUser(user.id)"
                        class="button-update-delete button-update-delete-hover-black">DELETE</button>
                </td>
            </tr>
        </table>
    </div>
</template>

<script>
import * as userAPI from '@/services/API/userAPI';

export default {
    data() {
        return {
            users: [],
            searchedUsers: [],
            inputUserEmail: "",
            user: {id : 0}
        }
    },
    methods: {
        async enterUserCreatePage() {
            this.$router.push('/createUser');
        },


        async searchUser() {
            this.searchedUsers = this.users.filter(user => user.email.includes(this.inputUserEmail));
        },

        async deleteUser(id) {
            const token = localStorage.getItem('token');
            await userAPI.deleteUser(id, token);

            this.users = await userAPI.getAllUsers(token);
            this.users = this.users.sort((a, b) => a.role.localeCompare(b.role));
            this.users = this.users.filter(user => user.id !== this.user.id);
            this.searchedUsers = this.users;
            this.inputUserEmail = "";
        },
    },

    async mounted() {
        const token = localStorage.getItem('token');
        if (token) {
            this.users = await userAPI.getAllUsers(token);
            this.user = await userAPI.getUserByToken(token);
            this.users = this.users.sort((a, b) => a.role.localeCompare(b.role));
            this.users = this.users.filter(user => user.id !== this.user.id);
            this.searchedUsers = this.users;
        }else{
            this.$router.push('/login');
        }

    }
}
</script>

<style>
@import "./../assets/css/styleTable.css";
</style>