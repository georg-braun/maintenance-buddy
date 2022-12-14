import { makeGetRequest, sendPost } from './api-service';

class ApiServer {
	public async getVehicles() {
		try {
			const response = await makeGetRequest('vehicles');
			if (response?.data == undefined) return [];
			return response.data;
		} catch (error) {
			console.log(error);
		}
	}

	public async getVehicle(vehicleId) {
		try {
			const response = await makeGetRequest(`vehicles/find/?vehicleId=${vehicleId}`);
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

	public async deleteVehicle(vehicleId){
		console.log(`Delete ${vehicleId}`)
		return makeGetRequest(`vehicle/delete/?vehicleId=${vehicleId}`);
	}

	public async addActionTemplate(vehicleId, name, kilometerInterval, timeIntervalInDays) {
		const data = {
			VehicleId: vehicleId,
			Name: name,
			KilometerInterval: kilometerInterval,
			TimeIntervalInDays: timeIntervalInDays
		};
		console.log(data)
		const response = await sendPost('action-template/create', data);
		if (response.status === 201 || response.status === 200) console.log('action template added');
	}

	public async deleteSchedule(vehicleId, scheduleId) {
		const data = {
			VehicleId: vehicleId,
			ActionTemplateId: scheduleId,
		};
		return sendPost('action-template/delete', data);
	}

	public async changeScheduleName(vehicleId, scheduleId, name) {
		const data = {
			VehicleId: vehicleId,
			ActionTemplateId: scheduleId,
			Name: name
		};
		return await sendPost('action-template/rename', data);
	}

	public async changeScheduleKilometerInterval(vehicleId, scheduleId, kilometer) {
		const data = {
			VehicleId: vehicleId,
			ActionTemplateId: scheduleId,
			KilometerInterval: kilometer
		};
		return await sendPost('action-template/change-kilometer-interval', data);
	}

	public async changeScheduleTimeInterval(vehicleId, scheduleId, dayx) {
		const data = {
			VehicleId: vehicleId,
			ActionTemplateId: scheduleId,
			TimeIntervalInDays: dayx
		};
		return await sendPost('action-template/change-time-interval', data);
	}

	public async addAction(vehicleId, actionTemplateId, date, kilometer, note) {
		const data = {
			VehicleId: vehicleId,
			ActionTemplateId: actionTemplateId,
			Date: date,
			Kilometer: kilometer,
			Note: note
		};

		console.log(data)
		const response = await sendPost('action/create', data);
		if (response.status === 201 || response.status === 200) console.log('action template added');
	}

	public async changeActionNote(vehicleId, actionTemplateId, actionId, note) {
		const data = {
			VehicleId: vehicleId,
			ActionTemplateId: actionTemplateId,
			ActionId: actionId,
			Note: note
		};

		return await sendPost('action/change-note', data);
	}

	public async changeActionKilometer(vehicleId, actionTemplateId, actionId, kilometer) {
		const data = {
			VehicleId: vehicleId,
			ActionTemplateId: actionTemplateId,
			ActionId: actionId,
			Kilometer: kilometer
		};

		console.log(data)
		return await sendPost('action/change-kilometer', data);
	}

	public async changeActionDate(vehicleId, actionTemplateId, actionId, date) {
		const data = {
			VehicleId: vehicleId,
			ActionTemplateId: actionTemplateId,
			ActionId: actionId,
			Date: date
		};

		return await sendPost('action/change-date', data);
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

	public async changeVehicleName(vehicleId, name) {
		const data = {
			VehicleId: vehicleId,
			Name: name
		};
		return sendPost('vehicle/rename', data);
	}

	public async changeVehicleKilometer(vehicleId, kilometer) {
		const data = {
			VehicleId: vehicleId,
			Kilometer: kilometer
		};
		return sendPost('vehicle/change-kilometer', data);
	}

	public async getPendingActions(vehicleId) {
		try {
			const response = await makeGetRequest(`vehicle/pending-actions/?vehicleId=${vehicleId}`);
			if (response?.data == undefined) return [];
			return response.data;
		} catch (error) {
			console.log(error);
		}		
	}
}

export default ApiServer;
