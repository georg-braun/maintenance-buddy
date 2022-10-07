import { makeGetRequest, sendPost } from './api-service';


class FakeServer {

	vehicles = [{ id: 1, name: "Opel", kilometer: 50000 },
	{ id: 2, name: "BMW", kilometer: 33000 }]

	actionTemplates = { 1: [{ id: 1, name: "Oil exchange", KilometerInterval: 5000 }, 
	{ id: 2, name: "Lights", KilometerInterval: 50000 }] }

	public async getVehicles() {
		return this.vehicles;
	}

	public async getActionTemplates(vehicleId) {
		return this.actionTemplates[vehicleId];
	}

	public async getActions(vehicleId) {

	}

	public async createVehicle(name, kilometer) {
		this.vehicles.push({name: name, kilometer: kilometer})
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


export default FakeServer;
