import { makeGetRequest, sendPost } from './api-service';

class ApiServer {
	public async getVehicles() {
		try {
			const response = await makeGetRequest('get-vehicles');
			if (response?.data == undefined)
				return [];
			return response.data;
			
		} catch (error) {
			console.log(error);
		}
	}
	
	public async getActionTemplates(vehicleId) {
		try {
			const response = await makeGetRequest(`get-action-templates/?vehicleId=${vehicleId}`);
			return response.data;
		} catch (error) {
			console.log(error);
		}
	}
	
	public async getActions(vehicleId) {
		try {
			const response = await makeGetRequest(`get-actions-of-vehicle/?vehicleId=${vehicleId}`);
			return response.data;
		} catch (error) {
			console.log(error);
		}
	}
	
	public async createVehicle(name, kilometer) {
		const data = {
			Name: name,
			Kilometer: kilometer
		};
		const response = await sendPost('create-vehicle', data);
		if (response.status === 201 || response.status === 200) console.log('vehicle created');
	}
	
	public async addActionTemplate(vehicleId, name, kilometerInterval, timeInterval) {
		const data = {
			VehicleId: vehicleId,
			Name: name,
			KilometerInterval: kilometerInterval
		};
		const response = await sendPost('add-action-template', data);
		if (response.status === 201 || response.status === 200) console.log('action template added');
	}
	
	public async addAction(vehicleId, actionTemplateId, date, kilometer, note) {
		const data = {
			VehicleId: vehicleId,
			ActionTemplateId: actionTemplateId,
			Date: date,
			Kilometer: kilometer,
			Note: note
		};
		const response = await sendPost('add-action', data);
		if (response.status === 201 || response.status === 200) console.log('action template added');
	}
}

export default ApiServer