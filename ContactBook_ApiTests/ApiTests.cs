using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;

namespace ContactBook_ApiTests
{
    public class ApiTests
    {
        private const string url = "https://contactbook.nakov.repl.co/api";
        private RestClient client;  
        private RestRequest request;

        [SetUp]
        public void Setup()
        {
            this.client = new RestClient(); 
        }

        [Test]
        public void TestGetListContactsCheckFirstContact()
        {
            request = new RestRequest(url+ "/contacts");

            var response = client.Execute(request);
            var contacts = JsonSerializer.Deserialize<List<Contact>>(response.Content);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(contacts[0].firstName, Is.EqualTo("Steve"));
            Assert.That(contacts[0].lastName, Is.EqualTo("Jobs"));
        }

        [Test]
        public void TestSearchContact_CheckFirstResult()
        {
            request = new RestRequest(url + "/contacts/search/albert");

            var response = client.Execute(request);
            var contacts = JsonSerializer.Deserialize<List<Contact>>(response.Content);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(contacts[0].firstName, Is.EqualTo("Albert"));
            Assert.That(contacts[0].lastName, Is.EqualTo("Einstein"));
        }

        [Test]
        public void TestSearch_EmptyResult()
        {
            request = new RestRequest(url + "/contacts/search/{keyword}");
            request.AddUrlSegment("keyword", "missing29");

            var response = client.Execute(request);
            var contacts = JsonSerializer.Deserialize<List<Contact>>(response.Content);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(contacts.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestAddContactWithInvalidParamaters()
        {
            request = new RestRequest(url + "/contacts");
            var body = new
            {
                firstName = "Bobi",
                email = "bbab@abv.bg",
                phone = "011133334"
            };
            request.AddJsonBody(body);  
            var response = client.Execute(request, Method.Post);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(response.Content.Contains("errMsg"));
         
        }

        [Test]
        public void TestAddContactWithValidParamaters()
        {
            request = new RestRequest(url + "/contacts");
            var body = new
            {
                firstName = "Bobi",
                lastName = "Bobov",
                email = "bbab@abv.bg",
                phone = "011133334"
            };
            request.AddJsonBody(body);
            var response = client.Execute(request, Method.Post);          
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            var allContacts = client.Execute(request, Method.Get);
            var contacts = JsonSerializer.Deserialize<List<Contact>>(allContacts.Content);
            Assert.That(contacts[contacts.Count - 1].firstName, Is.EqualTo(body.firstName));
            Assert.That(contacts[contacts.Count - 1].lastName, Is.EqualTo(body.lastName));


        }
    }
}