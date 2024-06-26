﻿using Models.DTO.People;
using Models.People;
using Newtonsoft.Json;
using Repositories.People;
using System.Net;
using System.Runtime.ConstrainedExecution;
using static System.Net.WebRequestMethods;

namespace Services.People;

public class AddressService
{
    private AddressRepository _addressRepository;


    public AddressService()
    {
        _addressRepository = new AddressRepository();
    }


    public List<Address> Get(string technology)
    {
        return _addressRepository.Get(technology);
    }


    public Address Get(string technology, int id)
    {
        return _addressRepository.Get(technology, id);
    }


    public bool Post(string technology, Address address)
    {
        address.PostalCode = address.PostalCode.Replace("-", "");
        return _addressRepository.Post(technology, address);
    }

    public async Task<Address> GetAddressByPostalCode(string cep)
    {
        dynamic responseAsDynamic = null;
        try
        {
            using (HttpClient client = new HttpClient())
            {
                string viacep = "https://viacep.com.br/ws";
                string url = $"{viacep}/{cep}/json/";

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    responseAsDynamic = JsonConvert.DeserializeObject<dynamic>(json);
                }
                else
                {
                    return null;
                }
            }
        }
        catch (Exception)
        {
            return null;
        }
        
        if (responseAsDynamic == null)
            return null;

        Address address = new Address()
        {
            Street = responseAsDynamic.logradouro,
            District = responseAsDynamic.bairro,
            State = responseAsDynamic.uf,
            City = responseAsDynamic.localidade,
        };
        return address;
    }


    public Address PostMongo(Address address)
    {
        return _addressRepository.PostMongo(address);
    }

}
