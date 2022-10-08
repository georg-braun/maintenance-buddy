import { makeGetRequest, sendPost } from './api-service';

class ApiServer {
	public async getVehicles() {
		try {
			const response = await makeGetRequest('vehicle');
			if (response?.data == undefined) return [];
			return response.data;
		} catch (error) {
			console.log(error);
		}
	}

	public async getActionTemplates(vehicleId) {
		try {
			const response = await makeGetRequest(`action-template/?vehicleId=${vehicleId}`);
			return response.data;
		} catch (error) {
			console.log(error);
		}
	}

	public async getActions(vehicleId) {
		try {
			const response = await makeGetRequest(`action/by-vehicle/?vehicleId=${vehicleId}`);
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
		const response = await sendPost('vehicle/create', data);
		if (response.status === 201 || response.status === 200) console.log('vehicle created');
	}

	public async addActionTemplate(vehicleId, name, kilometerInterval, timeInterval) {
		const data = {
			VehicleId: vehicleId,
			Name: name,
			KilometerInterval: kilometerInterval
		};
		const response = await sendPost('action-template/create', data);
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
		const response = await sendPost('action/create', data);
		if (response.status === 201 || response.status === 200) console.log('action template added');
	}

	public async deleteAction(vehicleId, actionTemplateId, actionId) {
		const data = {
			VehicleId: vehicleId,
			ActionTemplateId: actionTemplateId,
			ActionId: actionId
		};
		const response = await sendPost('action/delete', data);
		if (response.status === 201 || response.status === 200) console.log('action deleted');
	}
}

export default ApiServer;