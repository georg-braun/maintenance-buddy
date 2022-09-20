import axios from 'axios';
import auth from './auth-service';

import { env } from "$env/dynamic/public"

const serverUrl = env.PUBLIC_API_SERVER_ADDRESS;



async function makeRequest(config) {
	try {
		const token = await auth.getAccessToken();
		config.headers = {
			...config.headers,
			Authorization: `Bearer ${token}`
		};

		const response = axios.request(config);
		return response;
	} catch (error) {
		console.log(error);
	}
}

export async function sendPost(endpoint, data) {
	try {
		const token = await auth.getAccessToken();
		const config = {
			url: `${serverUrl}/api/${endpoint}`,
			method: 'POST',
			headers: {
				'content-type': 'application/json',
				Authorization: `Bearer ${token}`
			}
		};

		try {
			const response = await axios.post(`${serverUrl}/api/${endpoint}`, data, config);
		return response;
		} catch (error) {
			console.log(`${error.response.status}: ${error.response.data}`);
			return error.response;
		}

	} catch (error) {
		console.log(error);
	}
}


export async function getVehicles() {
	try {
		const config = {
			url: `${serverUrl}/api/get-vehicles`,
			method: 'GET',
			headers: {
				'content-type': 'application/json'
			}
		};
		const response = await makeRequest(config);
	
        console.log(response)

		if (response.data !== undefined)
			console.log(response.data);
	} catch (error) {
		console.log(error);
	}
}
