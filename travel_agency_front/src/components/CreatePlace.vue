<template>
    <h1 style="margin-top: 30px;">Create Place</h1>
    <div class="create-form">
        <input type="text" v-model="name" placeholder="Enter place name">
        <select id="country" v-model="country">
            <option v-for="country in countries" :key="country" :value="country">
                {{ country }}
            </option>
        </select><br>
        <textarea v-model="description" placeholder="Enter place description"></textarea>
        <button @click="createPlace">Create Place</button>
    </div>

</template>

<script>
import { countries } from '@/js/countries';
import * as placeAPI from '@/services/API/placeAPI';

export default {
    data() {
        return {
            name: '',
            country: '',
            description: '',
            countries: countries
        }
    },

    methods: {
        async createPlace() {
            const data = {
                name: this.name,
                country: this.country,
                description: this.description,
                siteUrl: 'hello',
            }

            await placeAPI.createPlace(data);
        }
    }
}
</script>

<style>

@import "./../assets/css/styleCreateForm.css";

</style>