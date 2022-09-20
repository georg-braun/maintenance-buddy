import { makeGetRequest } from "./api-service"

export async function getVehicles() {
	try {
		
		const response = await makeGetRequest("get-vehicles");
	
        console.log(response)

		if (response.data !== undefined)
			console.log(response.data);
	} catch (error) {
		console.log(error);
	}
}
