import React from 'react';
import Style from './Home.module.scss';
import me from '../../img/self.png';
import classNames from 'classnames';
import EmojiBullet from "./EmojiBullet";
import SocialIcon from "./SocialIcon";
import { Box } from "@mui/material";
import { info } from "../../info/Info";

import Swal from 'sweetalert2'
import api from "../../lib/api";
import { useState } from "react";
import loading from "../../img/loading.gif";

export default function Home() {
   const [isLoading, setIsLoading] = useState(false);

   const openModal = async () => {
      let { value: file } = await Swal.fire({
         title: 'UPLOAD CV',
         html: `<label class="swal2-label">Password:</label><input type="password" id="password" class="swal2-input" placeholder="Password">`,
         input: 'file',
         confirmButtonText: 'UPLOAD',
         cancelButtonText: 'CANCEL',
         cancelButtonColor: '#d33',
         showCancelButton: true,
      })
      
      if (file) {
         const password = Swal.getPopup().querySelector('#password').value;
         uploadCurriculo(password, file);
      }
   };

   const uploadCurriculo = (password, file) => {
      setIsLoading(true);

      const formData = new FormData();
      formData.append("Password", password);
      formData.append("File", file);

      api
         .post("/Upload", formData)
         .then((response) => {
            setIsLoading(false);
            Swal.fire({
               icon: 'success',
               title: 'Upload Success'
            })
         })
         .catch((err) => {
            setIsLoading(false);
            console.log(err);
            Swal.fire({
               icon: 'error',
               title: 'Upload Fail'
             })
         })
   };

   const dowloadCurriculo = () => {
      setIsLoading(true);
      api
         .post("/Download")
         .then((response) => {
            setIsLoading(false);
            window.open(response.data.url, '_blank');
         })
         .catch((err) => {
            setIsLoading(false);
            console.log(err);
         })
   };

   return (
      <>
         {isLoading &&
            <div style={{
               width: '100%',
               height: '80%',
               position: 'fixed',
               display: 'flex',
               alignItems: 'center',
               justifyContent: 'center',
               backdropFilter: 'blur(2px)',
               zIndex: '1000'
            }}>
               <img style={{ width: '500px' }} src={loading} alt="Carregando" />
            </div>
         }

         <Box component={'main'} display={'flex'} flexDirection={{ xs: 'column', md: 'row' }} alignItems={'center'}
            justifyContent={'center'} minHeight={'calc(100vh - 175px)'} mt={3}>

            <span id="teste" onClick={openModal}>
               <Box className={classNames(Style.avatar, Style.shadowed)} style={{ background: info.gradient }} component={'img'} src={me}
                  borderRadius={'50%'} p={'0.75rem'} mb={{ xs: '1rem', sm: 0 }} mr={{ xs: 0, md: '2rem' }} />
            </span>

            <Box>
               <h1>Hi, I'm <span style={{ background: info.gradient, webkitBackgroundClip: 'text', webkitTextFillColor: 'transparent' }}>{info.firstName}</span><span className={Style.hand}>🤚</span>
               </h1>
               <h2>I'm {info.position}.</h2>

               <Box component={'ul'} p={'0.8rem'}>
                  {info.miniBio.map(bio => (
                     <EmojiBullet emoji={bio.emoji} text={bio.text} />
                  ))}
               </Box>

               <Box display={'flex'} gap={'1.5rem'} justifyContent={'center'} fontSize={{ xs: '2rem', md: '2.5rem' }}>
                  {info.socials.map(social => (
                     <SocialIcon link={social.link} icon={social.icon} />
                  ))}
               </Box>

               <Box onClick={dowloadCurriculo}
                  display={'flex'} justifyContent={'center'} alignItems={'center'}
                  p={2} mt={3} border={'2px solid black'} borderRadius={'25px'} style={{ cursor: 'pointer' }}>
                  <h2>
                     <span style={{ background: info.gradient, webkitBackgroundClip: 'text', webkitTextFillColor: 'transparent' }}>
                        Baixar currículo
                     </span>
                  </h2>
               </Box>
            </Box>
         </Box>
      </>
   )
}