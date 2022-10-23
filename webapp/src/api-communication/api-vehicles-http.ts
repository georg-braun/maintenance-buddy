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
		const response = await sendPost('action-template/delete', data);
	}

	public async changeScheduleName(vehicleId, scheduleId, name) {
		const data = {
			VehicleId: vehicleId,
			ActionTemplateId: scheduleId,
			Name: name
		};
		const response = await sendPost('action-template/rename', data);
	}

	public async changeScheduleKilometerInterval(vehicleId, scheduleId, kilometer) {
		const data = {
			VehicleId: vehicleId,
			ActionTemplateId: scheduleId,
			KilometerInterval: kilometer
		};
		const response = await sendPost('action-template/change-kilometer-interval', data);
	}

	public async changeScheduleTimeInterval(vehicleId, scheduleId, dayx) {
		const data = {
			VehicleId: vehicleId,
			ActionTemplateId: scheduleId,
			TimeIntervalInDays: dayx
		};
		const response = await sendPost('action-template/change-time-interval', data);
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

	public async changeVehicleName(vehicleId, name) {
		const data = {
			VehicleId: vehicleId,
			Name: name
		};
		await sendPost('vehicle/rename', data);
	}

	public async changeVehicleKilometer(vehicleId, kilometer) {
		const data = {
			VehicleId: vehicleId,
			Kilometer: kilometer
		};
		await sendPost('vehicle/change-kilometer', data);
	}
}

export default ApiServer;
