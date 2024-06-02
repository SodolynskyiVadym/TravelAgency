<template>
    <h1>Create Hotel</h1>
    <label for="search">Search:</label>
    <input list="data" id="search" type="search" v-model="searchTerm" @input="test" />
    <datalist id="data">
        <option v-for="item in dataList" :key="item" :value="item">
            {{ item }}
        </option>
    </datalist>
</template>

<script>
import { countries } from '@/js/countries';
export default {
    data() {
        return {
            searchTerm: '',
            dataList: countries
        }
    },

    methods: {
        async test() {
            console.log(this.searchTerm);
        }
    }
}

</script>