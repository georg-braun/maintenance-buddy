import { goto } from '$app/navigation';

const base = window.location.origin

function gotoInternal(path){
    return goto(base + "/" + path)
}

export function goToAllVehicles(){
    return gotoInternal("")
}
