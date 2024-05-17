export default class Utils {

    public static async getBase64(e : File): Promise<string> {
        let result: string = ''

        let fileReader: FileReader = new FileReader()
        fileReader.readAsDataURL(e)

        let base64: Promise<any> = new Promise<any>((resolve, reject) => {
            fileReader.onload = () => resolve(fileReader.result)
            fileReader.onerror = () =>  reject()
        })
        
        await base64
        .then(x => result = x)
        .catch(x => result = String(x))

        return result
    }
}